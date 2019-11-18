using System;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.CommonCore
{
    public class VisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public VisibleReCaptchaValidateAttribute(
            IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration,
            IWebHostEnvironment hostingEnvironment)
            : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.ReCaptchaSecret = new Lazy<string>(() => this.GoogleRecaptchaKeys.VisibleRecaptcha.SecretKey);
        }
    }
}