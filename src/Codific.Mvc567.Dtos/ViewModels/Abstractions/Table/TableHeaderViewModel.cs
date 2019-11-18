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

using System.Collections.Generic;
using System.Linq;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions.Table
{
    public class TableHeaderViewModel
    {
        private List<TableHeaderCellViewModel> cells = new List<TableHeaderCellViewModel>();
        private int currentCellIndex = 0;

        public TableHeaderViewModel()
        {
        }

        public List<TableHeaderCellViewModel> Cells
        {
            get
            {
                return cells.OrderBy(x => x.Order).ToList();
            }
        }

        public void AddCell(string name)
        {
            this.cells.Add(new TableHeaderCellViewModel
            {
                Order = this.currentCellIndex,
                Name = name
            });

            this.currentCellIndex++;
        }
    }
}