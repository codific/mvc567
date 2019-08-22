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
using Mvc567.Entities.Database;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mvc567.Services.Infrastructure
{
    public interface ISignInService
    {
        Task<IEnumerable<Claim>> GetUserClaimsAsync(User user);

        Task<bool> UserHasAdministrationAccessRightsAsync(User user);

        Task<SignInResult> SignInAsync(User user, string password, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties);

        Task<SignInResult> SignInWith2faAsync(User user, string authenticationCode, bool rememberBrowser, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties);

        Task<User> GetTwoFactorAuthenticationUserAsync(HttpContext httpContext);

        Task SignOutAsync(HttpContext httpContext);
    }
}
