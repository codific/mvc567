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

using Microsoft.AspNetCore.Identity;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codific.Mvc567.Entities.Database
{
    public class Role : IdentityRole<Guid>, IAuditableEntityBase
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public bool Deleted { get; set; }
        
        public DateTime? DeletedOn { get; set; }
        
        public string DeletedBy { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime UpdatedOn { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}
