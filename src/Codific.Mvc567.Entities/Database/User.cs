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
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using System;

namespace Codific.Mvc567.Entities.Database
{
    public class User : IdentityUser<Guid>, IEntityBase
    {
        [SearchCriteria]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        [SearchCriteria]
        public string FirstName { get; set; }

        [SearchCriteria]
        public string LastName { get; set; }

        [SearchCriteria]
        public override string Email { get => base.Email; set => base.Email = value; }

        public DateTime RegistrationDate { get; set; }

        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
