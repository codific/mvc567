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

namespace Mvc567.Common.Enums
{
    public enum TableCellType
    {
        Text = 1,
        Date = 2,
        Time = 3,
        DateTime = 4,
        Number = 5,
        File = 6,
        Flag = 7,
        TextArea = 8
    }

    public enum CreateEntityInputType
    {
        Text = 1,
        TextArea = 2,
        Email = 3,
        Password = 4,
        Date = 5,
        Time = 6,
        File = 7,
        Integer = 8,
        Double = 9,
        EnumSelect = 10,
        EnumCheckbox = 11,
        EnumRadio = 12,
        DatabaseSelect = 13,
        DatabaseCheckbox = 14,
        DatabaseRadio = 15,
        BoolSelect = 16,
        BoolRadio = 17,
        DatabaseTablesSelect = 18
    }
}
