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
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions.Table
{
    public class TableRowViewModel
    {
        private List<TableCellViewModel> cells = new List<TableCellViewModel>();
        private List<TableRowActionViewModel> actions = new List<TableRowActionViewModel>();

        public List<TableCellViewModel> Cells
        {
            get
            {
                return this.cells.OrderBy(x => x.Order).ToList();
            }
        }

        public List<TableRowActionViewModel> Actions
        {
            get
            {
                return this.actions;
            }
        }

        public string Identifier { get; set; }

        public void AddCell(int order, object content, TableCellType type, bool editable, string relatedProperty)
        {
            TableCellViewModel tableCell = new TableCellViewModel(order, content, type, editable, relatedProperty);
            this.cells.Add(tableCell);
        }

        public bool HasOrder(int order)
        {
            return this.cells.Where(x => x.Order == order).Any();
        }

        public void ReplaceContentOnOrder(int order, object content)
        {
            this.cells.Where(x => x.Order == order).FirstOrDefault()?.SetRawContent(content);
        }

        public void AddAction(TableRowActionViewModel action, List<string> parameters)
        {
            var newAction = new TableRowActionViewModel(
                action.Title,
                action.Icon,
                action.Color,
                action.UrlStringFormat,
                action.Parameters,
                action.Method);

            newAction.HasConfirmation = action.HasConfirmation;
            newAction.ConfirmationTitle = action.ConfirmationTitle;
            newAction.ConfirmationMessage = action.ConfirmationMessage;
            newAction.Parameters = parameters;

            this.actions.Add(newAction);
        }
    }
}
