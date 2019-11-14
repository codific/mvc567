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

using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codific.Mvc567.Entities.Database
{
    [Table("StaticPages")]
    public class StaticPage : AuditableEntityBase
    {
        public string Title { get; set; }

        public string Route { get; set; }

        public bool Active { get; set; }

        [Column(TypeName = "ntext")]
        public string HtmlContent { get; set; }

        [Column(TypeName = "ntext")]
        public string ScriptContent { get; set; }

        [Column(TypeName = "ntext")]
        public string ОbfuscatedScriptContent { get; set; }

        [Column(TypeName = "ntext")]
        public string StyleContent { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public Guid? ImageId { get; set; }

        [ForeignKey("ImageId")]
        [SaveDirectory(ApplicationRoots.Public, Constants.AssetsFolderName, Constants.ImagesFolderName)]
        public virtual File Image { get; set; }

        public Guid? ParentPageId { get; set; }

        [ForeignKey("ParentPageId")]
        public virtual StaticPage ParentPage { get; set; }

        public Guid LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
