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

using System.ComponentModel.DataAnnotations.Schema;
using Codific.Mvc567.Common.Attributes;

namespace Codific.Mvc567.Entities.Database
{
    [Table("Logs")]
    public class Log : AuditableEntityBase
    {
        [SearchCriteria]
        [Column(TypeName = "ntext")]
        public string StackTrace { get; set; }

        [SearchCriteria]
        public string Source { get; set; }

        [SearchCriteria]
        public string Message { get; set; }

        [SearchCriteria]
        public string Method { get; set; }

        [SearchCriteria]
        public string Class { get; set; }
    }
}
