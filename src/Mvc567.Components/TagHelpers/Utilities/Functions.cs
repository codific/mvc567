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

using Mvc567.DataAccess.Abstraction.Entities;
using System;
using System.Collections.Generic;

namespace Mvc567.Components.TagHelpers.Utilities
{
    public static class Functions
    {
        public static Dictionary<Guid, string> GetDatabaseEntityDictionary(IEnumerable<IEntityBase> databaseEntities, string visibleProperty)
        {
            var databaseEntitiesDictionary = new Dictionary<Guid, string>();
            foreach (var entity in databaseEntities)
            {
                databaseEntitiesDictionary[entity.Id] = entity.GetType().GetProperty(visibleProperty).GetValue(entity)?.ToString();
            }

            return databaseEntitiesDictionary;
        }
    }
}
