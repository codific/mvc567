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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common;
using Codific.Mvc567.Entities.ViewModels.Abstractions.Table;
using Codific.Mvc567.Dtos.ServiceResults;

namespace Codific.Mvc567.Entities.ViewModels.Mapping
{
    public static class TableMapper
    {
        public static TableViewModel DtoMapper<T>(PaginatedEntitiesResult<T> entitiesResult, params TableRowActionViewModel[] actions)
        {
            Type dtoType = typeof(T);
            TableViewModel tableViewModel = new TableViewModel();
            var properties = dtoType.GetProperties();
            List<TableCellAttribute> dtoAttributes = new List<TableCellAttribute>();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                {
                    dtoAttributes.Add((TableCellAttribute)property.GetCustomAttributes(typeof(TableCellAttribute), false).FirstOrDefault());
                }
                
            }

            List<string> tableHeaders = dtoAttributes.OrderBy(x => x.Order).Select(x => x.Name).Distinct().ToList();
            tableHeaders.ForEach(x =>
            {
                tableViewModel.Header.AddCell(x);
            });

            if (entitiesResult.EntitiesCount > 0)
            {
                foreach (var entity in entitiesResult.Entities)
                {
                    TableRowViewModel tableRow = new TableRowViewModel();
                    foreach (var property in properties)
                    {
                        if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                        {
                            TableCellAttribute propertyAttribute = (TableCellAttribute)property.GetCustomAttributes(typeof(TableCellAttribute), false).FirstOrDefault();
                            tableRow.AddCell(propertyAttribute.Order, property.GetValue(entity), propertyAttribute.Type);
                        }
                    }

                    foreach (var action in actions)
                    {
                        List<string> parsedParameters = new List<string>();
                        foreach (var parameter in action.RawParameters)
                        {
                            if (parameter.StartsWith("[", StringComparison.OrdinalIgnoreCase) && parameter.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                            {
                                string propertyName = parameter.Substring(1, parameter.Length - 2);
                                string propertyValue = entity.GetType().GetProperty(propertyName).GetValue(entity).ToString();
                                parsedParameters.Add(propertyValue);
                            }
                            else
                            {
                                parsedParameters.Add(parameter);
                            }
                        }

                        tableRow.AddAction(action, parsedParameters);
                    }

                    tableViewModel.AddRow(tableRow);
                }
            }

            tableViewModel.SetPagination(entitiesResult.CurrentPage, entitiesResult.Pages);
            
            return tableViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon">https://materialdesignicons.com/cdn/2.0.46/</param>
        /// <param name="color"></param>
        /// <param name="urlStringFormat"></param>
        /// <param name="parameters"></param>
        public static TableRowActionViewModel CreateAction(string title, string icon, Color color, TableRowActionMethod method, string urlStringFormat, params object[] parameters)
        {
            return new TableRowActionViewModel(title, icon, color, urlStringFormat, parameters.Select(x => x.ToString()).ToList(), method);
        }

        public static TableRowActionViewModel DetailsAction(string urlStringFormat, params object[] parameters)
        {
            return CreateAction("Details", MaterialDesignIcons.Details, Color.DodgerBlue, TableRowActionMethod.Get, urlStringFormat, parameters);
        }

        public static TableRowActionViewModel EditAction(string urlStringFormat, params object[] parameters)
        {
            return CreateAction("Edit", MaterialDesignIcons.Pencil, Color.Orange, TableRowActionMethod.Get, urlStringFormat, parameters);
        }

        public static TableRowActionViewModel DeleteAction(string urlStringFormat, params object[] parameters)
        {
            var action = CreateAction("Delete", MaterialDesignIcons.Delete, Color.OrangeRed, TableRowActionMethod.Post, urlStringFormat, parameters);
            action.SetConfirmation("Delete Entity", "Are you sure you want to delete this entity?");

            return action;
        }
    }
}
