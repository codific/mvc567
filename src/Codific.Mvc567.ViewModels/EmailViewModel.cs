﻿using AutoMapper;
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

        [TableCell(6, "Body", TableCellType.TextArea)]
        [DetailsOrder(6)]
        public string EmailBody { get; set; }

        [TableCell(2, "Sent", TableCellType.Flag)]
        [DetailsOrder(2)]
        public bool Sent { get; set; }

        [TableCell(3, "Receiver Name", TableCellType.Text)]
        [DetailsOrder(3)]
        public string ReceiverName { get; set; }

        [TableCell(4, "Receiver Email", TableCellType.Text)]
        [DetailsOrder(4)]
        public string ReceiverEmail { get; set; }

        [TableCell(5, "Type", TableCellType.Text)]
        [DetailsOrder(5)]
        public string Type { get; set; }
    }
}
