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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Mvc567.DataAccess.Identity;

namespace Mvc567.Middlewares
{
    public class SwaggerAdminValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAdminValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.StartsWith("/swagger") &&
                !(httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole(UserRoles.Admin)))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SwaggerAdminValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerAdminValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerAdminValidationMiddleware>();
        }
    }
}
