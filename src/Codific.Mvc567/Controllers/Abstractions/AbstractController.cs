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
using System.Linq;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Dtos.Entities;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Codific.Mvc567.Controllers.Abstractions
{
    public abstract class AbstractController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        private readonly ILanguageService languageService;

        public AbstractController(IConfiguration configuration, IEmailService emailService, ILanguageService languageService)
        {
            this.configuration = configuration;
            this.emailService = emailService;
            this.languageService = languageService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.PreventInvalidLanguage(ref context);
            this.ManageLanguageCookie();
            base.OnActionExecuting(context);
        }

        protected void ManageLanguageCookie()
        {
            string languageCode = this.HttpContext.GetLanguageCode();
            var defaultLanguage = this.languageService.GetDefaultLanguage<SimpleLanguageDto>();
            if (defaultLanguage == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = defaultLanguage.Code.ToLower();
            }

            bool cookieExist = this.HttpContext.Request.Cookies.ContainsKey(Constants.LanguageCookieName);

            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(1),
                IsEssential = true,
            };

            if (cookieExist)
            {
                this.Response.Cookies.Delete(Constants.LanguageCookieName);
            }

            this.Response.Cookies.Append(Constants.LanguageCookieName, languageCode, options);
        }

        protected void PreventInvalidLanguage(ref ActionExecutingContext context)
        {
            string[] allowedLanguages = this.languageService.GetAllLanguageCodes();
            string[] filteredAllowedLanguages = allowedLanguages;
            string languageCode = this.HttpContext.GetLanguageCode();
            var defaultLanguage = this.languageService.GetDefaultLanguage<SimpleLanguageDto>();
            if (defaultLanguage != null)
            {
                filteredAllowedLanguages = allowedLanguages.Where(x => x != defaultLanguage.Code.ToLower()).ToArray();
            }

            if (!string.IsNullOrEmpty(languageCode) && !filteredAllowedLanguages.Contains(languageCode))
            {
                context.Result = this.NotFound();
            }
        }
    }
}
