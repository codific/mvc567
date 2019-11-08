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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Codific.Mvc567.Middlewares
{
    public class AdminRedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRedirectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (!httpContext.User.Identity.IsAuthenticated &&
                httpContext.Request.Path.Value.StartsWith("/admin") &&
                !httpContext.Request.Path.Value.StartsWith("/admin/login") &&
                !httpContext.Request.Path.Value.StartsWith("/admin/auth/init") &&
                !httpContext.Request.Path.Value.StartsWith("/admin/lockout"))
            {
                httpContext.Response.Redirect("/admin/login");
                return;
            }

            await _next.Invoke(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AdminRedirectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminRedirection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminRedirectionMiddleware>();
        }
    }
}
