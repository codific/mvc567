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
using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Codific.Mvc567.Common.Attributes
{
    public abstract class ReCaptchaValidateAttribute : ActionFilterAttribute
    {
        private const string ReCaptchaModelErrorKey = "ReCaptcha";
        private const string RecaptchaResponseTokenKey = "g-recaptcha-response";
        private const string ApiVerificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";
        private readonly GoogleRecaptchaKeys googleRecaptchaKeys;
        private readonly IHostingEnvironment hostingEnvironment;
        private Lazy<string> reCaptchaSecret;

        public ReCaptchaValidateAttribute(
            IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration,
            IHostingEnvironment hostingEnvironment)
        {
            this.googleRecaptchaKeys = googleRecaptchaKeysConfiguration.Value;
            this.hostingEnvironment = hostingEnvironment;
        }

        public Lazy<string> ReCaptchaSecret
        {
            get { return this.reCaptchaSecret; }
            set { this.reCaptchaSecret = value; }
        }

        public GoogleRecaptchaKeys GoogleRecaptchaKeys => this.googleRecaptchaKeys;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await this.DoReCaptchaValidation(context);
            await base.OnActionExecutionAsync(context, next);
        }

        private static void AddModelError(ActionExecutingContext context, string error)
        {
            context.ModelState.AddModelError(ReCaptchaModelErrorKey, error.ToString());
        }

        private async Task DoReCaptchaValidation(ActionExecutingContext context)
        {
            await Task.Run(() => { });

            // if (this.hostingEnvironment.IsDevelopment())
            // {
            //     return;
            // }
            //
            // if (!context.HttpContext.Request.HasFormContentType)
            // {
            //     AddModelError(context, "No reCaptcha Token Found");
            //     return;
            // }
            //
            // string token = context.HttpContext.Request.Form[RecaptchaResponseTokenKey];
            // if (string.IsNullOrWhiteSpace(token))
            // {
            //     AddModelError(context, "No reCaptcha Token Found");
            // }
            // else
            // {
            //     await this.ValidateRecaptcha(context, token);
            // }
        }

        private async Task ValidateRecaptcha(ActionExecutingContext context, string token)
        {
            using (var webClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", this.reCaptchaSecret.Value),
                    new KeyValuePair<string, string>("response", token),
                });
                HttpResponseMessage response = await webClient.PostAsync(ApiVerificationEndpoint, content);
                string json = await response.Content.ReadAsStringAsync();
                var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
                if (reCaptchaResponse == null)
                {
                    AddModelError(context, "Unable To Read Response From Server");
                }
                else if (!reCaptchaResponse.Success)
                {
                    AddModelError(context, "Invalid reCaptcha");
                }
            }
        }
    }
}