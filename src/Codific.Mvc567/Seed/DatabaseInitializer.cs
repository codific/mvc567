// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
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
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Codific.Mvc567.Seed
{
    public class DatabaseInitializer<TDatabaseContext> : IApplicationDatabaseInitializer
        where TDatabaseContext : AbstractDatabaseContext<TDatabaseContext>
    {
        private readonly TDatabaseContext context;
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identityService;

        private Dictionary<string, string[]> additionalRoles = null;

        public DatabaseInitializer(TDatabaseContext context, UserManager<User> userManager, IIdentityService identityService)
        {
            this.context = context;
            this.userManager = userManager;
            this.identityService = identityService;
        }

        public void LoadAdditionalRoles(Dictionary<string, string[]> roles)
        {
            this.additionalRoles = roles;
        }

        public async Task SeedAsync()
        {
            await this.context.Database.MigrateAsync();

            if (!await this.context.Roles.AnyAsync())
            {
                await this.EnsureRoleAsync(UserRoles.Admin, UserRoles.Admin, ApplicationPermissions.GetAllPermissionValues());
                await this.EnsureRoleAsync(UserRoles.User, UserRoles.User, new string[] { });
                if (this.additionalRoles != null && this.additionalRoles.Count > 0)
                {
                    foreach (var role in this.additionalRoles)
                    {
                        await this.EnsureRoleAsync(role.Key, role.Key, role.Value);
                    }
                }
            }

            if (!await this.context.Users.AnyAsync())
            {
                await this.CreateUserAsync("admin@example.com", "Admin123!", "AdminFirst", "AdminLast", new string[] { UserRoles.Admin.ToString() });
                await this.CreateUserAsync("user@example.com", "User123!", "UserFirst", "UserLast", new string[] { UserRoles.User.ToString() });
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (!(await this.context.Roles.Where(x => x.Name == roleName).AnyAsync()))
            {
                Role role = new Role(roleName, description);
                var result = await this.identityService.CreateRoleAsync(role, claims);
            }
        }

        private async Task<User> CreateUserAsync(string email, string password, string firstName, string lastName, string[] roles)
        {
            User user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                RegistrationDate = DateTime.Now,
            };

            var result = await this.userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await this.userManager.AddToRolesAsync(user, roles);
            }

            return user;
        }
    }
}
