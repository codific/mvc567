using System;
using Microsoft.AspNetCore.Identity;

namespace Codific.Mvc567.Providers
{
    public class ResetPasswordTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public const string TokenProviderName = "DefaultResetPasswordTokenProviderOptions";

        public ResetPasswordTokenProviderOptions()
        {
            this.Name = TokenProviderName;
            this.TokenLifespan = TimeSpan.FromDays(7);
        }
    }
}