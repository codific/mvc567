using System;
using System.ComponentModel.DataAnnotations;

namespace Codific.Mvc567.Dtos.ViewModels.AdminViewModels
{
    public class AdminCreateNewUserViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Guid Role { get; set; }

        public Type RoleType { get; set; }
    }
}
