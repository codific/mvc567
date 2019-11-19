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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Codific.Mvc567.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetLanguageCode(this HttpContext httpContext)
        {
            var route = httpContext.GetRouteData();
            object language = string.Empty;
            string languageCode = string.Empty;
            route.Values.TryGetValue(Constants.LanguageControllerRouteKey, out language);
            if (language != null)
            {
                languageCode = language.ToString().ToLower();
            }

            return languageCode;
        }

        public static Guid? GetJwtUserId(this HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated && httpContext.User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Jti).Any())
            {
                return Guid.Parse(httpContext.User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Jti).Select(x => x.Value).FirstOrDefault());
            }

            return null;
        }
    }
}
