// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
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

using AutoMapper;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Entities.Database;
using System;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(Log), ReverseMap = true)]
    public class LogViewModel
    {
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableCell(1, "Message", TableCellType.TextArea)]
        [DetailsOrder(1)]
        public string Message { get; set; }

        [TableCell(2, "Stack Trace", TableCellType.TextArea)]
        [DetailsOrder(5)]
        public string StackTrace { get; set; }

        [TableCell(3, "Source", TableCellType.Text)]
        [DetailsOrder(2)]
        public string Source { get; set; }

        [TableCell(4, "Method", TableCellType.Text)]
        [DetailsOrder(3)]
        public string Method { get; set; }

        [TableCell(5, "Class", TableCellType.Text)]
        [DetailsOrder(4)]
        public string Class { get; set; }

        [TableCell(6, "Created On", TableCellType.Date)]
        [DetailsOrder(6)]
        public DateTime CreatedOn { get; set; }

        [TableCell(7, "Created By", TableCellType.Text)]
        [DetailsOrder(7)]
        public string CreatedBy { get; set; }
    }
}
