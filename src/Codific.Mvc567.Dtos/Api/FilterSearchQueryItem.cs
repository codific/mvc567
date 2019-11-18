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

using Codific.Mvc567.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codific.Mvc567.Dtos.Api
{
    public class FilterSearchQueryItem
    {
        private readonly Dictionary<string, FilterSearchQueryEqualityType> equalitySymbolsDictionary = new Dictionary<string, FilterSearchQueryEqualityType>
        {
            { "=", FilterSearchQueryEqualityType.Equals },
            { "~=", FilterSearchQueryEqualityType.Contains },
            { ">", FilterSearchQueryEqualityType.GreaterThan },
            { "<", FilterSearchQueryEqualityType.LessThan },
            { ">=", FilterSearchQueryEqualityType.GreaterOrEqualsThan },
            { "<=", FilterSearchQueryEqualityType.LessOrEqualsThan },
            { "!=", FilterSearchQueryEqualityType.NotEqual }
        };

        public FilterSearchQueryItem(string propertyName, string equalitySymbol, string value)
        {
            PropertyName = propertyName;
            EqualityType = this.equalitySymbolsDictionary.ContainsKey(equalitySymbol) ? this.equalitySymbolsDictionary[equalitySymbol] : FilterSearchQueryEqualityType.Equals;
            Value = value;
        }

        public string PropertyName { get; private set; }

        public FilterSearchQueryEqualityType EqualityType { get; private set; }

        public string Value { get; private set; }
    }
}
