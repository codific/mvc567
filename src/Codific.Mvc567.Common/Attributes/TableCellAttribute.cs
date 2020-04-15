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
using System.Runtime.CompilerServices;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Attributes
{
    public class TableCellAttribute : Attribute
    {
        public TableCellAttribute(int order, string name, TableCellType type)
        {
            this.Order = order;
            this.Name = name;
            this.Type = type;
        }

        public TableCellAttribute(
            int order,
            string name,
            TableCellType type,
            string textForTrueValue,
            string textForFalseValue,
            [CallerMemberName] string propertyName = null)
        {
            this.PropertyName = propertyName;
            this.Order = order;
            this.Name = name;
            this.Type = type;
            this.TextForFalseValue = textForFalseValue;
            this.TextForTrueValue = textForTrueValue;
        }

        public string TextForTrueValue { get; private set; }

        public string TextForFalseValue { get; private set; }

        public int Order { get; private set; }

        public string Name { get; private set; }

        public TableCellType Type { get; private set; }

        public bool Editable { get; set; } = true;

        public string PropertyName { get; set; }
    }
}
