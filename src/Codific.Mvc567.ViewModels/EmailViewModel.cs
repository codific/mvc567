using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(Email), ReverseMap = true)]
    public class EmailViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [DetailsOrder(6)]
        [HtmlContent]
        public string EmailBody { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [TableCell(2, "Sent", TableCellType.Flag)]
        [DetailsOrder(2)]
        public bool Sent { get; set; }

        [SortableProperty]
        [TableCell(3, "Receiver Name", TableCellType.Text)]
        [DetailsOrder(3)]
        public string ReceiverName { get; set; }

        [SortableProperty]
        [TableCell(4, "Receiver Email", TableCellType.Text)]
        [DetailsOrder(4)]
        public string ReceiverEmail { get; set; }

        [SortableProperty]
        [TableCell(5, "Type", TableCellType.Text)]
        [DetailsOrder(5)]
        public string Type { get; set; }
    }
}
