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

using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(SystemConstant), ReverseMap = true)]
    public class SystemConstantViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableCell(1, "Key", TableCellType.Text, Editable = true)]
        [DetailsOrder(1)]
        [Required(ErrorMessage = "Key is required field.")]
        [CreateEditEntityInput("Key", CreateEntityInputType.Text)]
        public string Key { get; set; }

        [TableCell(2, "Value", TableCellType.Text, Editable = true)]
        [DetailsOrder(2)]
        [Required(ErrorMessage = "Value is required field.")]
        [CreateEditEntityInput("Value", CreateEntityInputType.Text)]
        public string Value { get; set; }

        [TableCell(3, "Type", TableCellType.Text)]
        [DetailsOrder(3)]
        [Required(ErrorMessage = "Type is required field.")]
        [CreateEditEntityInput("Type", CreateEntityInputType.EnumSelect)]
        public SystemConstantTypes Type { get; set; }

        [TableCell(4, "Private", TableCellType.Flag)]
        [DetailsOrder(4)]
        [CreateEditEntityInput("Private", CreateEntityInputType.BoolRadio)]
        public bool Private { get; set; }
    }
}