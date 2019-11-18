using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Codific.Mvc567.CommonCore
{
    public abstract class ReCaptchaValidateAttribute : ActionFilterAttribute
    {
        private const string ReCaptchaModelErrorKey = "ReCaptcha";
        private const string RecaptchaResponseTokenKey = "g-recaptcha-response";
        private const string ApiVerificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";
        private readonly GoogleRecaptchaKeys googleRecaptchaKeys;
        private readonly IWebHostEnvironment hostingEnvironment;
        private Lazy<string> reCaptchaSecret;

        public ReCaptchaValidateAttribute(
            IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration,
            IWebHostEnvironment hostingEnvironment)
        {
            this.googleRecaptchaKeys = googleRecaptchaKeysConfiguration.Value;
            this.hostingEnvironment = hostingEnvironment;
        }

        public Lazy<string> ReCaptchaSecret
        {
            get
            {
                return this.reCaptchaSecret;
            }

            set
            {
                this.reCaptchaSecret = value;
            }
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
                await this.ValidateRecaptcha(context, token);
            }
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