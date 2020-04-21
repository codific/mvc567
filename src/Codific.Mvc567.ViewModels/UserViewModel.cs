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
    [AutoMap(typeof(User), ReverseMap = true)]
    public class UserViewModel
    {
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [TableCell(1, "Email", TableCellType.Text, Editable = false)]
        [DetailsOrder(1)]
        public string Email { get; set; }

        [DetailsOrder(2)]
        public string FirstName { get; set; }

        [DetailsOrder(3)]
        public string LastName { get; set; }

        [SortableProperty]
        [TableCell(2, "Name", TableCellType.Text, Editable = false)]
        public string Name
        {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }

        [SortableProperty]
        [TableCell(5, "Registration", TableCellType.DateTime)]
        [DetailsOrder(6)]
        public DateTime RegistrationDate { get; set; }

        [SortableProperty]
        [TableCell(3, "2FA", TableCellType.Flag)]
        [DetailsOrder(4)]
        public bool TwoFactorEnabled { get; set; }

        [SortableProperty]
        [TableCell(4, "Locked Out", TableCellType.Flag)]
        [DetailsOrder(5)]
        public bool IsLockedOut { get; set; }
    }
}