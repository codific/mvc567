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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codific.Mvc567.Entities.Database
{
    [Table("Languages")]
    public class Language : EntityBase
    {
        [SearchCriteria]
        public string Code { get; set; }

        [SearchCriteria]
        public string Name { get; set; }

        [SearchCriteria]
        public string NativeName { get; set; }

        public Guid ImageId { get; set; }

        [ForeignKey("ImageId")]
        [SaveDirectory(ApplicationRoots.Public, Constants.AssetsFolderName, Constants.ImagesFolderName)]
        public virtual File Image { get; set; }

        [SearchCriteria]
        public string TranslationFileUrl { get; set; }

        public DateTime? LastTranslationFileGeneration { get; set; }

        public virtual ICollection<TranslationValue> Translations { get; set; }

        public bool IsDefault { get; set; }
    }
}
