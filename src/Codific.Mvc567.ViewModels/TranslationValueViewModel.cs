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

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.ViewModels
{
    [AutoMap(typeof(TranslationValue), ReverseMap = true)]
    public class TranslationValueViewModel : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Language is required field.")]
        [CreateEditEntityInput("Language", CreateEntityInputType.DatabaseRadio)]
        [DatabaseEnum(typeof(Language), "Name")]
        public string LanguageId { get; set; }

        [TableDefaultOrderProperty(FilterOrderType.Ascending)]
        [SortableProperty]
        [DetailsOrder(1)]
        [TableCell(1, "Language", TableCellType.Text)]
        [Ignore]
        public string LanguageName
        {
            get
            {
                return this.Language?.Name;
            }
        }

        public LanguageViewModel Language { get; set; }

        [Required(ErrorMessage = "Translation Key is required field.")]
        [CreateEditEntityInput("Translation Key", CreateEntityInputType.DatabaseSelect)]
        [DatabaseEnum(typeof(TranslationKey), "Key")]
        public string TranslationKeyId { get; set; }

        public TranslationKeyViewModel TranslationKey { get; set; }

        [SortableProperty]
        [DetailsOrder(2)]
        [TableCell(2, "Translation Key", TableCellType.Text)]
        [Ignore]
        public string TranslationKeyValue
        {
            get
            {
                return this.TranslationKey?.Key;
            }
        }

        [SortableProperty]
        [DetailsOrder(3)]
        [TableCell(3, "Value", TableCellType.Text)]
        [Required(ErrorMessage = "Value is required field.")]
        [CreateEditEntityInput("Value", CreateEntityInputType.Text)]
        public string Value { get; set; }
    }
}
