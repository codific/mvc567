using System;
using Codific.Mvc567.Dtos.EmailModels.Abstraction;

namespace Codific.Mvc567.Dtos.EmailModels
{
    public class UserResetPasswordEmailModel : AbstractEmailModel
    {
        public string resetPasswordUrl { get; set; }
    }
}