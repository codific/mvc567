using System;
using System.ComponentModel.DataAnnotations;

namespace Codific.Mvc567.Dtos.ViewModels.AdminViewModels
{
    public class AdminResetPasswordViewModel
    {
        public string PasswordResetToken { get; set; }

        public Guid UserId { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Password should has: at least 8 characters, uppercase character, lowercase character, special symbol, numeric character!")]
        [Required(ErrorMessage = "New Password field is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Confirmed Password must match with New Password field.")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}