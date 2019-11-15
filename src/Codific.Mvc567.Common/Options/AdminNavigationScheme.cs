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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Codific.Mvc567.Common.Options
{
    public class AdminNavigationScheme
    {
        private const string schemeFileName = "adminmenus.json";

        public AdminNavigationScheme(string contentRootPath)
        {
            string schemeContentString = File.ReadAllText(Path.Combine(contentRootPath, schemeFileName));
            AdminNavigationScheme sheme = JsonConvert.DeserializeObject<AdminNavigationScheme>(schemeContentString);
            this.Menus = sheme.Menus;
        }

        public AdminNavigationScheme()
        {

        }

        [JsonProperty("navigation")]
        public List<SidebarMenuSectionItem> Menus { get; set; }
    }
}
