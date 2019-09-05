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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mvc567.Common.Utilities;
using System;

namespace Mvc567.Common.Attributes
{
    public class ValidateAdminCookieAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            var hostingEnvornment = (IHostingEnvironment)context.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment));
            string[] cookieValues = CookiesFunctions.GenerateAdminLoginCookieValues(
                configuration["AdminLoginAuthenticator:CookieFormat"], 
                new string[] { context.HttpContext.Request.Headers["User-Agent"], DateTime.Now.Year.ToString() },
                configuration["AdminLoginAuthenticator:SecretIndexes"]);

            if (!hostingEnvornment.IsDevelopment() || Convert.ToBoolean(configuration["AdminLoginAuthenticator:ShowForTestPurposes"]))
            {
                if (!(context.HttpContext.Request.Cookies.ContainsKey(cookieValues[0]) || context.HttpContext.Request.Cookies[cookieValues[0]] == cookieValues[1]))
                {
                    context.Result = new NotFoundResult();

                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
