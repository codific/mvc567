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
using Codific.Mvc567.Dtos.ViewModels.Abstractions;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(Language), ReverseMap = true)]
    public class LanguageViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableCell(1, "Code", TableCellType.Text)]
        [DetailsOrder(1)]
        [CreateEditEntityInput("Code", CreateEntityInputType.Text)]
        public string Code { get; set; }

        [TableCell(2, "Name", TableCellType.Text)]
        [DetailsOrder(2)]
        [CreateEditEntityInput("Name", CreateEntityInputType.Text)]
        public string Name { get; set; }

        [TableCell(3, "Native Name", TableCellType.Text)]
        [DetailsOrder(3)]
        [CreateEditEntityInput("Native Name", CreateEntityInputType.Text)]
        public string NativeName { get; set; }

        [DetailsOrder(4, Title = "Image")]
        [TableCell(4, "Image", TableCellType.File)]
        [CreateEditEntityInput("Image", CreateEntityInputType.File)]
        public string ImageId { get; set; }
        
        public string TranslationFileUrl { get; set; }

        [TableCell(5, "Last Generation", TableCellType.DateTime)]
        [DetailsOrder(5)]
        public DateTime? LastTranslationFileGeneration { get; set; }

        [TableCell(6, "Is Default", TableCellType.Flag)]
        [DetailsOrder(6)]
        [CreateEditEntityInput("Is Default", CreateEntityInputType.BoolSelect)]
        public bool IsDefault { get; set; }
    }
}
