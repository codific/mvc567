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
    [AutoMap(typeof(SidebarMenuSectionItem), ReverseMap = true)]
    public class SidebarMenuSectionItemViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableCell(1, "Title", TableCellType.Text)]
        [DetailsOrder(1)]
        [Required(ErrorMessage = "Title is required field.")]
        [CreateEditEntityInput("Title", CreateEntityInputType.Text)]
        public string Title { get; set; }

        [TableCell(2, "Controller", TableCellType.Text)]
        [DetailsOrder(2)]
        [Required(ErrorMessage = "Controller is required field.")]
        [CreateEditEntityInput("Controller", CreateEntityInputType.Text)]
        public string ItemController { get; set; }

        [TableCell(3, "Action", TableCellType.Text)]
        [DetailsOrder(3)]
        [CreateEditEntityInput("Action", CreateEntityInputType.Text)]
        public string ItemAction { get; set; }

        [TableCell(4, "Area", TableCellType.Text)]
        [DetailsOrder(4)]
        [CreateEditEntityInput("Area", CreateEntityInputType.Text)]
        public string ItemArea { get; set; }

        [TableCell(4, "Order", TableCellType.Text)]
        [DetailsOrder(4)]
        [Required(ErrorMessage = "Order is required field.")]
        [CreateEditEntityInput("Order", CreateEntityInputType.Integer)]
        public int Order { get; set; }

        [TableCell(5, "Single", TableCellType.Flag)]
        [DetailsOrder(5)]
        [CreateEditEntityInput("Single", CreateEntityInputType.BoolRadio)]
        public bool Single { get; set; }

        [TableCell(6, "Icon", TableCellType.Text)]
        [DetailsOrder(6)]
        [CreateEditEntityInput("Icon", CreateEntityInputType.Text)]
        public string Icon { get; set; }

        [CreateEditEntityInput("AdminNavigationSchemeId", CreateEntityInputType.Hidden)]
        public Guid AdminNavigationSchemeId { get; set; }
    }
}
