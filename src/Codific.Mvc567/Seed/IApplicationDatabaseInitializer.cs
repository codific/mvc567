﻿// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
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

using System.Collections.Generic;

namespace Codific.Mvc567.Seed
{
    public interface IApplicationDatabaseInitializer : IDatabaseInitializer
    {
        /// <summary>
        /// Load additonal roles for the database initializer
        /// </summary>
        /// <param name="roles">Dictionary that contains role name as a key and array of role permissions (string array) as a value.</param>
        void LoadAdditionalRoles(Dictionary<string, string[]> roles);
    }
}
