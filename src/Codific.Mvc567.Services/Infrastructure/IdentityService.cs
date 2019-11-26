// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class IdentityService : Service, IIdentityService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;

        public IdentityService(
            IUnitOfWork uow,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
            : base(uow, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<Tuple<bool, string[]>> CreateRoleAsync<TRole>(TRole role, IEnumerable<string> claims)
        {
            var entityRole = this.Mapper.Map<Role>(role);

            if (claims == null)
            {
                claims = new string[] { };
            }

            string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            if (invalidClaims.Any())
            {
                return Tuple.Create(false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });
            }

            var result = await this.roleManager.CreateAsync(entityRole);
            if (!result.Succeeded)
            {
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
            }

            entityRole = await this.roleManager.FindByNameAsync(entityRole.Name);

            foreach (string claim in claims.Distinct())
            {
                result = await this.roleManager.AddClaimAsync(entityRole, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                if (!result.Succeeded)
                {
                    await this.DeleteRoleAsync(role);
                    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName)
        {
            var role = await this.roleManager.FindByNameAsync(roleName);

            if (role != null)
            {
                return await this.DeleteRoleAsync(role);
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync<TRole>(TRole role)
        {
            var entityRole = this.Mapper.Map<Role>(role);

            var result = await this.roleManager.DeleteAsync(entityRole);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<TUserModel> GetUserByEmailAsync<TUserModel>(string email)
        {
            try
            {
                User user = await this.userManager.FindByEmailAsync(email);
                TUserModel resultUser = this.Mapper.Map<TUserModel>(user);

                return resultUser;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetUserByEmailAsync));
                return default(TUserModel);
            }
        }

        public async Task<IEnumerable<TUserModel>> GetAllUsersAsync<TUserModel>()
        {
            try
            {
                var users = await this.StandardRepository.GetAllAsync<User>();
                var usersResult = this.Mapper.Map<IEnumerable<TUserModel>>(users);

                return usersResult;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetAllUsersAsync));
                return null;
            }
        }

        public async Task<TUserModel> GetUserByIdAsync<TUserModel>(Guid userId)
        {
            try
            {
                User user = await this.StandardRepository.GetAsync<User>(userId);
                TUserModel resultUser = this.Mapper.Map<TUserModel>(user);

                return resultUser;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetUserByIdAsync));

                return default(TUserModel);
            }
        }
    }
}