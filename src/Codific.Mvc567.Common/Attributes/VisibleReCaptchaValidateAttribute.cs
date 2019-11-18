using System;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.Common.Attributes
{
    public class VisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public VisibleReCaptchaValidateAttribute(
            IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration,
            IHostingEnvironment hostingEnvironment)
            : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.ReCaptchaSecret = new Lazy<string>(() => this.GoogleRecaptchaKeys.VisibleRecaptcha.SecretKey);
        }
    }
}