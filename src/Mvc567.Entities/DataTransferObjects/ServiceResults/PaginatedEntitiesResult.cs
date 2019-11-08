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

using System.Collections.Generic;
using System.Linq;

namespace Mvc567.Entities.DataTransferObjects.ServiceResults
{
    public class PaginatedEntitiesResult<TEntity>
    {
        public IEnumerable<TEntity> Entities { get; set; }

        public int Count { get; set; }

        public int EntitiesCount
        {
            get
            {
                return (Entities != null) ? Entities.Count() : 0;
            }
        }

        public int Pages {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }

                return (Count / PageSize) + (Count % PageSize == 0 ? 0 : 1);
            }
        }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int StartRow
        {
            get
            {
                return ((CurrentPage - 1) * PageSize);
            }
        }
    }
}
