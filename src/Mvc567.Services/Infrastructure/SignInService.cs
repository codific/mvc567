// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Georgi Karagogov
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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Mvc567.DataAccess.Identity;
using Mvc567.Entities.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mvc567.Services.Infrastructure
{
    public class SignInService : ISignInService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private const string TwoFactorAuthenticationTokenProvider = "Authenticator";

        public SignInService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
        {
            if (user != null)
            {
                var claims = await this.userManager.GetClaimsAsync(user);
                var userRoles = await this.userManager.GetRolesAsync(user);
                foreach (var roleName in userRoles)
                {
                    Role currentRole = await this.roleManager.FindByNameAsync(roleName);
                    var roleClaims = await this.roleManager.GetClaimsAsync(currentRole);
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.Email));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

                return claims.ToList();
            }

            return null;
        }

        public async Task<SignInResult> SignInAsync(User user, string password, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties)
        {
            if (await this.userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            var claims = await GetUserClaimsAsync(user);
            if (claims == null)
            {
                return SignInResult.Failed;
            }

            var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);

            SignInResult signInResult = SignInResult.Failed;
            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, password);
            if (isPasswordCorrect)
            {
                if (!user.TwoFactorEnabled && !user.IsLockedOut)
                {
                    await httpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                    signInResult = SignInResult.Success;
                }
                else if (user.TwoFactorEnabled)
                {
                    await httpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, StoreTwoFactorInfo(user.Id, null));
                    signInResult = SignInResult.TwoFactorRequired;
                }
                else if (user.IsLockedOut)
                {
                    signInResult = SignInResult.LockedOut;
                }
            }
            else
            {
                await this.userManager.AccessFailedAsync(user);
            }

            return signInResult;
        }

        private ClaimsPrincipal StoreTwoFactorInfo(Guid userId, string loginProvider)
        {
            var identity = new ClaimsIdentity(IdentityConstants.TwoFactorUserIdScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));
            if (loginProvider != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, loginProvider));
            }
            return new ClaimsPrincipal(identity);
        }

        public async Task<bool> UserHasAdministrationAccessRightsAsync(User user)
        {
            bool userHasAdministrationAccessRights = false;
            var userClaims = await GetUserClaimsAsync(user);
            if (userClaims == null)
            {
                return false;
            }
            if (userClaims.Where(x => x.Type == CustomClaimTypes.Permission && x.Value == ApplicationPermissions.AccessAdministration).Any())
            {
                userHasAdministrationAccessRights = true;
            }

            return userHasAdministrationAccessRights;
        }

        private async Task<TwoFactorAuthenticationInfo> RetrieveTwoFactorInfoAsync(HttpContext httpContext)
        {
            var result = await httpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (result?.Principal != null)
            {
                return new TwoFactorAuthenticationInfo
                {
                    UserId = result.Principal.FindFirstValue(ClaimTypes.Name),
                    LoginProvider = result.Principal.FindFirstValue(ClaimTypes.AuthenticationMethod)
                };
            }
            return null;
        }

        public async Task<User> GetTwoFactorAuthenticationUserAsync(HttpContext httpContext)
        {
            var info = await RetrieveTwoFactorInfoAsync(httpContext);
            if (info == null)
            {
                return null;
            }

            return await this.userManager.FindByIdAsync(info.UserId);
        }

        public async Task<SignInResult> SignInWith2faAsync(User user, string authenticationCode, bool rememberBrowser, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties)
        {
            if (await this.userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            var claims = await GetUserClaimsAsync(user);
            if (claims == null)
            {
                return SignInResult.Failed;
            }

            var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);

            SignInResult signInResult = SignInResult.Failed;
            var isAuthenticationCodeCorrect = await this.userManager.VerifyTwoFactorTokenAsync(user, TwoFactorAuthenticationTokenProvider, authenticationCode);
            if (isAuthenticationCodeCorrect)
            {
                if (!user.IsLockedOut)
                {
                    await httpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                    signInResult = SignInResult.Success;
                    if (rememberBrowser)
                    {
                        await this.signInManager.RememberTwoFactorClientAsync(user);
                    }
                }
                else
                {
                    signInResult = SignInResult.LockedOut;
                }
            }
            else
            {
                await this.userManager.AccessFailedAsync(user);
            }

            return signInResult;
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
    }

    internal class TwoFactorAuthenticationInfo
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
    }
}
