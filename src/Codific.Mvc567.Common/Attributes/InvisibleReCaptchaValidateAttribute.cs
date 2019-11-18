using System;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.Common.Attributes
{
    public class InvisibleReCaptchaValidateAttribute : ReCaptchaValidateAttribute
    {
        public InvisibleReCaptchaValidateAttribute(IOptions<GoogleRecaptchaKeys> googleRecaptchaKeysConfiguration, IHostingEnvironment hostingEnvironment)
            : base(googleRecaptchaKeysConfiguration, hostingEnvironment)
        {
            this.ReCaptchaSecret = new Lazy<string>(() => this.GoogleRecaptchaKeys.InvisibleRecaptcha.SecretKey);
        }
    }
}