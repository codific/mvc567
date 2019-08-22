// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Georgi Karagogov
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
using Mvc567.Common.Enums;
using Mvc567.Common.Attributes;
using Mvc567.Entities.Database;
using System.ComponentModel.DataAnnotations;
using Mvc567.Entities.ViewModels.Abstractions;

namespace Mvc567.Entities.DataTransferObjects.Entities
{
    [AutoMap(typeof(SitemapItemPattern), ReverseMap = true)]
    public class SitemapItemPatternDto : CreateEditEntityViewModel
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [TableCell(2, "Single Page", TableCellType.Flag)]
        [DetailsOrder(2)]
        [CreateEditEntityInput("Single Page", CreateEntityInputType.BoolRadio)]
        public bool SinglePage { get; set; }

        [TableCell(1, "Pattern", TableCellType.Text)]
        [DetailsOrder(1)]
        [CreateEditEntityInput("Pattern", CreateEntityInputType.Text)]
        [Required(ErrorMessage = "Pattern field is required.")]
        public string Pattern { get; set; }

        [TableCell(3, "Related Entity", TableCellType.Text)]
        [DetailsOrder(3)]
        [CreateEditEntityInput("Related Entity", CreateEntityInputType.DatabaseTablesSelect)]
        public string RelatedEntity { get; set; }

        [TableCell(4, "Priority", TableCellType.Number)]
        [DetailsOrder(4)]
        [CreateEditEntityInput("Priority", CreateEntityInputType.Double)]
        public float Priority { get; set; }

        [TableCell(5, "Change Frequency", TableCellType.Text)]
        [DetailsOrder(5)]
        [CreateEditEntityInput("Change Frequency", CreateEntityInputType.EnumSelect)]
        public SeoChangeFrequencyTypes ChangeFrequency { get; set; }
    }
}
