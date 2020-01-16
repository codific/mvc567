using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(User), ReverseMap = true)]
    public class AdminEditUserViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        public Guid Id { get; set; }

        [Required]
        [CreateEditEntityInput("First Name", CreateEntityInputType.Text)]
        public string FirstName { get; set; }

        [Required]
        [CreateEditEntityInput("Last Name", CreateEntityInputType.Text)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [CreateEditEntityInput("Email", CreateEntityInputType.Email)]
        public string Email { get; set; }
    }
}
