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
    [AutoMap(typeof(SidebarNavigationLinkItem), ReverseMap = true)]
    public class SidebarNavigationLinkItemViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [TableCell(1, "Title", TableCellType.Text)]
        [DetailsOrder(1)]
        [Required(ErrorMessage = "Title is required field.")]
        [CreateEditEntityInput("Title", CreateEntityInputType.Text)]
        public string Title { get; set; }

        [SortableProperty]
        [TableCell(2, "Controller", TableCellType.Text)]
        [DetailsOrder(2)]
        [Required(ErrorMessage = "Controller is required field.")]
        [CreateEditEntityInput("Controller", CreateEntityInputType.Text)]
        public string ItemController { get; set; }

        [SortableProperty]
        [TableCell(3, "Action", TableCellType.Text)]
        [DetailsOrder(3)]
        [Required(ErrorMessage = "Action is required field.")]
        [CreateEditEntityInput("Action", CreateEntityInputType.Text)]
        public string ItemAction { get; set; }

        [SortableProperty]
        [TableCell(4, "Area", TableCellType.Text)]
        [DetailsOrder(4)]
        [Required(ErrorMessage = "Area is required field.")]
        [CreateEditEntityInput("Area", CreateEntityInputType.Text)]
        public string ItemArea { get; set; }

        [SortableProperty]
        [TableCell(5, "Order", TableCellType.Text)]
        [DetailsOrder(5)]
        [Required(ErrorMessage = "Order is required field.")]
        [CreateEditEntityInput("Order", CreateEntityInputType.Integer)]
        public int Order { get; set; }

        [CreateEditEntityInput("ParentSectionId", CreateEntityInputType.Hidden)]
        public Guid ParentSectionId { get; set; }
    }
}
