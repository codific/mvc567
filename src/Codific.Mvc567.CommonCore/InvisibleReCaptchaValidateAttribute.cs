using System;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.CommonCore
{
    public class InvisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public InvisibleReCaptchaValidateAttribute(
            IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration,
            IWebHostEnvironment hostingEnvironment)
            : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.ReCaptchaSecret = new Lazy<string>(() => this.GoogleRecaptchaKeys.InvisibleRecaptcha.SecretKey);
        }
    }
}