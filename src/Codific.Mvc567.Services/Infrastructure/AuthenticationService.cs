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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class AuthenticationService : Codific.Mvc567.Services.Abstractions.IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private const string TwoFactorAuthenticationTokenProvider = "Authenticator";
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork uow;

        public AuthenticationService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IConfiguration configuration, IUnitOfWork uow)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.uow = uow;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync<TUser>(TUser user)
        {
            return await this.GetUserClaimsActionAsync<User>(user as User);
        }


        private async Task<IEnumerable<Claim>> GetUserClaimsActionAsync<TUser>(TUser user) where TUser : User
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

        public async Task<SignInResult> SignInAsync<TUser>(TUser user, string password, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties)
        {
            var parsedUser = user as User;
            if (await this.userManager.IsLockedOutAsync(parsedUser))
            {
                return SignInResult.LockedOut;
            }

            var claims = await GetUserClaimsAsync(parsedUser);
            if (claims == null)
            {
                return SignInResult.Failed;
            }

            var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);

            SignInResult signInResult = SignInResult.Failed;
            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(parsedUser, password);
            if (isPasswordCorrect)
            {
                if (!parsedUser.TwoFactorEnabled && !parsedUser.IsLockedOut)
                {
                    await httpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                    signInResult = SignInResult.Success;
                }
                else if (parsedUser.TwoFactorEnabled)
                {
                    await httpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, StoreTwoFactorInfo(parsedUser.Id, null));
                    signInResult = SignInResult.TwoFactorRequired;
                }
                else if (parsedUser.IsLockedOut)
                {
                    signInResult = SignInResult.LockedOut;
                }
            }
            else
            {
                await this.userManager.AccessFailedAsync(parsedUser);
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

        public async Task<bool> UserHasAdministrationAccessRightsAsync<TUser>(TUser user)
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

        public async Task<TUser> GetTwoFactorAuthenticationUserAsync<TUser>(HttpContext httpContext) where TUser : class
        {
            var info = await RetrieveTwoFactorInfoAsync(httpContext);
            if (info == null)
            {
                return default(TUser);
            }

            return await this.userManager.FindByIdAsync(info.UserId) as TUser;
        }

        public async Task<SignInResult> SignInWith2faAsync<TUser>(TUser user, string authenticationCode, bool rememberBrowser, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties) where TUser : class
        {
            var parsedUser = user as User;
            if (await this.userManager.IsLockedOutAsync(parsedUser))
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
            var isAuthenticationCodeCorrect = await this.userManager.VerifyTwoFactorTokenAsync(parsedUser, TwoFactorAuthenticationTokenProvider, authenticationCode);
            if (isAuthenticationCodeCorrect)
            {
                if (!parsedUser.IsLockedOut)
                {
                    await httpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                    signInResult = SignInResult.Success;
                    if (rememberBrowser)
                    {
                        await this.signInManager.RememberTwoFactorClientAsync(parsedUser);
                    }
                }
                else
                {
                    signInResult = SignInResult.LockedOut;
                }
            }
            else
            {
                await this.userManager.AccessFailedAsync(parsedUser);
            }

            return signInResult;
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        public async Task<BearerAuthResponse> BuildTokenAsync(string email, string password)
        {
            try
            {
                var user = await this.userManager.FindByEmailAsync(email);

                if (await this.userManager.IsLockedOutAsync(user))
                {
                    return BearerAuthResponse.FailedResult;
                }

                if (!(await this.userManager.CheckPasswordAsync(user, password)))
                {
                    await this.userManager.AccessFailedAsync(user);
                    return BearerAuthResponse.FailedResult;
                }

                if (user.EmailConfirmed)
                {
                    var userClaims = await GetAllUserClaimsAsync(user);
                    string jwt = BuildJwtToken(user, userClaims);
                    string refreshToken = $"{Guid.NewGuid().ToString().Replace("-", "0")}{Guid.NewGuid().ToString().Replace("-", "1")}";
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiration = DateTime.Now.AddMonths(1);
                    this.uow.GetStandardRepository().Update<User>(user);
                    await this.uow.SaveChangesAsync();

                    return BearerAuthResponse.SuccessResult(jwt, refreshToken);
                }
                
                return BearerAuthResponse.FailedResult;
            }
            catch (Exception)
            {
                return BearerAuthResponse.FailedResult;
            }
        }

        public async Task<BearerAuthResponse> RefreshTokenAsync(Guid? userId, string refreshToken)
        {
            try
            {
                User user = null;
                if (userId.HasValue)
                {
                    user = await this.userManager.FindByIdAsync(userId.ToString());
                }
                else
                {
                    user = (await this.uow.GetStandardRepository().QueryAsync<User>(x => x.RefreshToken == refreshToken)).FirstOrDefault();
                }

                if (user != null && user.RefreshToken == refreshToken && user.RefreshTokenExpiration.HasValue && user.RefreshTokenExpiration > DateTime.Now)
                {
                    var userClaims = await GetAllUserClaimsAsync(user);
                    string jwt = BuildJwtToken(user, userClaims);

                    return BearerAuthResponse.SuccessResult(jwt, refreshToken);
                }

                return BearerAuthResponse.FailedResult;
            }
            catch (Exception)
            {
                return BearerAuthResponse.FailedResult;
            }
        }

        private string BuildJwtToken(User user, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expirationDate;
            expirationDate = DateTime.Now.AddMinutes(15);

            var token = new JwtSecurityToken(this.configuration["Jwt:Issuer"],
              this.configuration["Jwt:Issuer"],
              claims,
              expires: expirationDate,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetAllUserClaimsAsync(User user)
        {
            var userClaims = await this.userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            var rolesClaims = new List<Claim>();
            var userRoles = await this.userManager.GetRolesAsync(user);
            foreach (string role in userRoles)
            {
                
                if (role != UserRoles.Admin)
                {
                    var currentRole = await this.roleManager.FindByNameAsync(role);
                    var currentRoleClaims = await this.roleManager.GetClaimsAsync(currentRole);
                    foreach (var currentRoleClaim in currentRoleClaims)
                    {
                        if (!userClaims.Where(x => x.Type == currentRoleClaim.Type && x.Value == currentRoleClaim.Value).Any())
                        {
                            userClaims.Add(currentRoleClaim);
                        }
                    }
                }
            }

            return userClaims?.ToList();
        }

        public async Task<bool> ResetUserRefreshTokensAsync(Guid userId)
        {
            try
            {
                var user = await this.userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiration = null;
                    this.uow.GetStandardRepository().Update<User>(user);
                    await this.uow.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    internal class TwoFactorAuthenticationInfo
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
    }
}
