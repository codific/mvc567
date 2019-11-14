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
    public class VisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public VisibleReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IWebHostEnvironment hostingEnvironment) : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.reCaptchaSecret = new Lazy<string>(() => this.googleRecaptchaKeys.VisibleRecaptcha.SecretKey);
        }
    }

    public class InvisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public InvisibleReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IWebHostEnvironment hostingEnvironment) : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
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
        private readonly IWebHostEnvironment hostingEnvironment;

        public ReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IWebHostEnvironment hostingEnvironment)
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
