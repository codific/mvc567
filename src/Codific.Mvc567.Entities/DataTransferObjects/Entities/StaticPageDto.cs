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
using System.ComponentModel.DataAnnotations;

namespace Codific.Mvc567.Entities.DataTransferObjects.Entities
{
    [AutoMap(typeof(StaticPage), ReverseMap = true)]
    public class StaticPageDto
    {
        [EntityIdentifier]
        [DetailsOrder(0)]
        public string Id { get; set; }

        [DetailsOrder(1)]
        [TableCell(1, "Title", TableCellType.Text)]
        [Required(ErrorMessage = "Title field is required.")]
        public string Title { get; set; }

        [DetailsOrder(2)]
        [TableCell(2, "Route", TableCellType.Text)]
        [Required(ErrorMessage = "Route field is required.")]
        public string Route { get; set; }

        [DetailsOrder(3)]
        [TableCell(3, "Active", TableCellType.Flag)]
        public bool Active { get; set; }

        public string HtmlContent { get; set; }

        public string ScriptContent { get; set; }

        public string ОbfuscatedScriptContent { get; set; }

        public string StyleContent { get; set; }

        [DetailsOrder(4)]
        [TableCell(4, "Description", TableCellType.TextArea)]
        public string Description { get; set; }

        [DetailsOrder(5)]
        [TableCell(5, "Keywords", TableCellType.TextArea)]
        public string Keywords { get; set; }

        [DetailsOrder(6, Title = "Image")]
        [TableCell(6, "Image", TableCellType.File)]
        public Guid? ImageId { get; set; }

        public virtual FileDto Image { get; set; }

        public Guid? ParentPageId { get; set; }

        public virtual StaticPageDto ParentPage { get; set; }

        [Required]
        public Guid LanguageId { get; set; }

        public virtual LanguageDto Language { get; set; }
    }
}
