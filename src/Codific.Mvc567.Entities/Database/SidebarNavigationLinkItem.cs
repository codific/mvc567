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

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codific.Mvc567.Entities.Database
{
    [Table("SidebarNavigationLinkItems")]
    public class SidebarNavigationLinkItem : EntityBase
    {
        public string Title { get; set; }

        public string ItemController { get; set; }

        public string ItemAction { get; set; }

        public string ItemArea { get; set; }

        public int Order { get; set; }

        public Guid? ParentSectionId { get; set; }

        [ForeignKey("ParentSectionId")]
        public virtual SidebarMenuSectionItem ParentSection { get; set; }
    }
}
