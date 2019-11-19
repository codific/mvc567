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

using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Dtos.Api
{
    public class FilterQueryOrderItem
    {
        public FilterQueryOrderItem(string propertyName, string orderSymbol)
        {
            this.PropertyName = propertyName;
            if (string.IsNullOrEmpty(orderSymbol) || orderSymbol == "a")
            {
                this.OrderType = FilterOrderType.Ascending;
            }
            else if (orderSymbol == "d")
            {
                this.OrderType = FilterOrderType.Descending;
            }
        }

        public string PropertyName { get; private set; }

        public FilterOrderType OrderType { get; private set; }
    }
}
