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

namespace Codific.Mvc567.Entities.ViewModels.Abstractions.Table
{
    public class TableViewModel
    {
        public TableViewModel()
        {
            Header = new TableHeaderViewModel();
            Rows = new List<TableRowViewModel>();
        }

        public TableHeaderViewModel Header { get; set; }

        public List<TableRowViewModel> Rows { get; private set; }

        public TablePaginationViewModel Pagination { get; set; }

        public bool HasActions
        {
            get
            {
                return Rows.FirstOrDefault() == null ? false : Rows.FirstOrDefault().Actions.Any();
            }
        }

        public void AddRow(TableRowViewModel row)
        {
            if (row != null)
            {
                Rows.Add(row);
            }
        }

        public void SetPagination(int currentPage, int pagesCount)
        {
            Pagination = new TablePaginationViewModel(currentPage, pagesCount);
        }

        public string Area { get; private set; }

        public string Controller { get; private set; }

        public string Action { get; private set; }

        public void SetPaginationRedirection(string area, string controller, string action)
        {
            Area = area;
            Controller = controller;
            Action = action;
        }
    }
}
