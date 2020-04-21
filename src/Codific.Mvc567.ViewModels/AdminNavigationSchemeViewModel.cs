using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(AdminNavigationScheme), ReverseMap = true)]
    public class AdminNavigationSchemeViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [TableCell(1, "Name", TableCellType.Text, Editable = true)]
        [DetailsOrder(1)]
        [Required(ErrorMessage = "Name is required field.")]
        [CreateEditEntityInput("Name", CreateEntityInputType.Text)]
        public string Name { get; set; }

        [SortableProperty(OrderArgument = "Role.Name")]
        [DetailsOrder(1)]
        [TableCell(1, "Role", TableCellType.Text)]
        [Ignore]
        public string RoleName
        {
            get
            {
                return this.Role?.Name;
            }
        }

        public RoleViewModel Role { get; set; }

        [CreateEditEntityInput("Role", CreateEntityInputType.DatabaseSelect)]
        [DatabaseEnum(typeof(Role), "Name")]
        public Guid? RoleId { get; set; }
    }
}
