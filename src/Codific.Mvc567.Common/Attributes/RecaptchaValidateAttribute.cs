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

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Codific.Mvc567.Common.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Codific.Mvc567.Common.Attributes
{
    public class VisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public VisibleReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IHostingEnvironment hostingEnvironment) : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.reCaptchaSecret = new Lazy<string>(() => this.googleRecaptchaKeys.VisibleRecaptcha.SecretKey);
        }
    }

    public class InvisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public InvisibleReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IHostingEnvironment hostingEnvironment) : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.reCaptchaSecret = new Lazy<string>(() => this.googleRecaptchaKeys.InvisibleRecaptcha.SecretKey);
        }
    }

    public abstract class ReCaptchaValidateAttribute : ActionFilterAttribute
    {
        public const string ReCaptchaModelErrorKey = "ReCaptcha";
        private const string RecaptchaResponseTokenKey = "g-recaptcha-response";
        private const string ApiVerificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";
        protected readonly GoogleRecaptchaKeys googleRecaptchaKeys;
        protected Lazy<string> reCaptchaSecret;
        private readonly IHostingEnvironment hostingEnvironment;

        public ReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IHostingEnvironment hostingEnvironment)
        {
            this.googleRecaptchaKeys = googleRecaptchaKeysConfiguration.Value;
            this.hostingEnvironment = hostingEnvironment;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await DoReCaptchaValidation(context);
            await base.OnActionExecutionAsync(context, next);
        }

        private async Task DoReCaptchaValidation(ActionExecutingContext context)
        {
            if (this.hostingEnvironment.IsDevelopment())
            {
                return;
            }

            if (!context.HttpContext.Request.HasFormContentType)
            {
                AddModelError(context, "No reCaptcha Token Found");
                return;
            }
            string token = context.HttpContext.Request.Form[RecaptchaResponseTokenKey];
            if (string.IsNullOrWhiteSpace(token))
            {
                AddModelError(context, "No reCaptcha Token Found");
            }
            else
            {
                await ValidateRecaptcha(context, token);
            }
        }
        private static void AddModelError(ActionExecutingContext context, string error)
        {
            context.ModelState.AddModelError(ReCaptchaModelErrorKey, error.ToString());
        }
        private async Task ValidateRecaptcha(ActionExecutingContext context, string token)
        {
            using (var webClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("secret", this.reCaptchaSecret.Value),
                        new KeyValuePair<string, string>("response", token)
                    });
                HttpResponseMessage response = await webClient.PostAsync(ApiVerificationEndpoint, content);
                string json = await response.Content.ReadAsStringAsync();
                var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
                if (reCaptchaResponse == null)
                {
                    AddModelError(context, "Unable To Read Response From Server");
                }
                else if (!reCaptchaResponse.success)
                {
                    AddModelError(context, "Invalid reCaptcha");
                }
            }
        }
    }

    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] errorcodes { get; set; }
    }
}
