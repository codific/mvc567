using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.Providers
{
    public class ResetPasswordTokenProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser : class
    {
            public ResetPasswordTokenProvider(
                IDataProtectionProvider dataProtectionProvider,
                IOptions<ResetPasswordTokenProviderOptions> options,
                ILogger<ResetPasswordTokenProvider<TUser>> logger)
                : base(dataProtectionProvider, options, logger)
            {
            }
    }
}