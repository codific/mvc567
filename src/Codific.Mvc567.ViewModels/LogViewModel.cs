// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(Log), ReverseMap = true)]
    public class LogViewModel
    {
        [DetailsOrder(0)]
        public string Id { get; set; }

        [SortableProperty]
        [TableCell(1, "Message", TableCellType.TextArea, Editable = false)]
        [DetailsOrder(1)]
        public string Message { get; set; }

        [SortableProperty]
        [TableCell(2, "Stack Trace", TableCellType.TextArea, Editable = false)]
        [DetailsOrder(5)]
        public string StackTrace { get; set; }

        [SortableProperty]
        [TableCell(3, "Source", TableCellType.Text, Editable = false)]
        [DetailsOrder(2)]
        public string Source { get; set; }

        [SortableProperty]
        [TableCell(4, "Method", TableCellType.Text, Editable = false)]
        [DetailsOrder(3)]
        public string Method { get; set; }

        [SortableProperty]
        [TableCell(5, "Class", TableCellType.Text, Editable = false)]
        [DetailsOrder(4)]
        public string Class { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [TableCell(6, "Created On", TableCellType.Date, Editable = false)]
        [DetailsOrder(6)]
        public DateTime CreatedOn { get; set; }

        [SortableProperty]
        [TableCell(7, "Created By", TableCellType.Text, Editable = false)]
        [DetailsOrder(7)]
        public string CreatedBy { get; set; }
    }
}
