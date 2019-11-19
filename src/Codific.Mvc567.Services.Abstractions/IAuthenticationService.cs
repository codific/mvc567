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
using System.Security.Claims;
using System.Threading.Tasks;
using Codific.Mvc567.Dtos.ServiceResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Codific.Mvc567.Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync<TUser>(TUser user);

        Task<bool> UserHasAdministrationAccessRightsAsync<TUser>(TUser user);

        Task<SignInResult> SignInAsync<TUser>(TUser user, string password, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties);

        Task<SignInResult> SignInWith2faAsync<TUser>(TUser user, string authenticationCode, bool rememberBrowser, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties)
            where TUser : class;

        Task<TUser> GetTwoFactorAuthenticationUserAsync<TUser>(HttpContext httpContext)
            where TUser : class;

        Task SignOutAsync(HttpContext httpContext);

        Task<BearerAuthResponse> BuildTokenAsync(string email, string password);

        Task<BearerAuthResponse> RefreshTokenAsync(Guid? userId, string refreshToken);

        Task<bool> ResetUserRefreshTokensAsync(Guid userId);
    }
}
