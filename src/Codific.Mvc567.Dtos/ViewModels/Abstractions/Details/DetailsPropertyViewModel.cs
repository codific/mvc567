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

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions.Details
{
    public class DetailsPropertyViewModel
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public TableCellType Type { get; set; }

        public int Order { get; set; }

        public bool IsHtml { get; set; }
    }
}
