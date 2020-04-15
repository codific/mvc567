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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Codific.Mvc567.Dtos.ViewModels.Mapping
{
    public static class TableMapper
    {
        public static TableViewModel DtoMapper<T>(PaginatedEntitiesResult<T> entitiesResult, params TableRowActionViewModel[] actions)
        {
            Type dtoType = typeof(T);
            TableViewModel tableViewModel = new TableViewModel();
            var properties = dtoType.GetProperties();
            List<TableCellAttribute> dtoAttributes = new List<TableCellAttribute>();
            string defaultOrderPropertyName = null;
            FilterOrderType? defaultOrderType = null;
            var tableCellAttributePropertyNames = new List<string>();
            var sortablePropertyAttributes = new List<SortablePropertyAttribute>();
            foreach (var property in properties)
            {
                var defaultOrderAttribute = (TableDefaultOrderPropertyAttribute)property.GetCustomAttributes(typeof(TableDefaultOrderPropertyAttribute), false).FirstOrDefault();
                if (defaultOrderAttribute != null)
                {
                    defaultOrderPropertyName = property.Name;
                    defaultOrderType = defaultOrderAttribute.OrderType;
                }

                if (property.GetCustomAttributes(typeof(SortablePropertyAttribute), false).Length > 0)
                {
                    var orderPropertyAttribute = (SortablePropertyAttribute)property.GetCustomAttributes(typeof(SortablePropertyAttribute), false).First();
                    tableCellAttributePropertyNames.Add(property.Name);
                    sortablePropertyAttributes.Add(orderPropertyAttribute);
                }
                else if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                {
                    sortablePropertyAttributes.Add(null);
                    tableCellAttributePropertyNames.Add(null);
                }

                if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                {
                    dtoAttributes.Add((TableCellAttribute)property.GetCustomAttributes(typeof(TableCellAttribute), false).FirstOrDefault());
                }
            }

            SortTableCellAttributesAndPropertyNamesByOrderWeight(dtoAttributes, tableCellAttributePropertyNames, sortablePropertyAttributes);
            var tableCellNames = dtoAttributes
                .Select(x => x.Name)
                .Distinct()
                .ToList();

            for (var i = 0; i < tableCellNames.Count; i++)
            {
                var propertyName = tableCellAttributePropertyNames[i];
                var tableCellName = tableCellNames[i];
                var orderPropertyAttribute = sortablePropertyAttributes[i];
                if (orderPropertyAttribute != null)
                {
                    if (propertyName == defaultOrderPropertyName)
                    {
                        tableViewModel.Header.AddCell(tableCellName, orderPropertyAttribute.OrderArgument, true, defaultOrderType);
                    }
                    else
                    {
                        tableViewModel.Header.AddCell(tableCellName, orderPropertyAttribute.OrderArgument, false);
                    }
                }
                else
                {
                    tableViewModel.Header.AddCell(tableCellName);
                }
            }

            if (entitiesResult.EntitiesCount > 0)
            {
                foreach (var entity in entitiesResult.Entities)
                {
                    TableRowViewModel tableRow = new TableRowViewModel();
                    foreach (var property in properties)
                    {
                        if (property.GetCustomAttributes(typeof(EntityIdentifierAttribute), false).Length > 0)
                        {
                            tableRow.Identifier = property.GetValue(entity)?.ToString();
                        }

                        if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                        {
                            TableCellAttribute propertyAttribute = (TableCellAttribute)property.GetCustomAttributes(typeof(TableCellAttribute), false).FirstOrDefault();

                            if (propertyAttribute?.Type == TableCellType.Flag)
                            {
                                tableRow.AddCell(
                                    propertyAttribute.Order,
                                    property.GetValue(entity),
                                    propertyAttribute.Type,
                                    propertyAttribute.Editable,
                                    property.Name,
                                    propertyAttribute.TextForTrueValue,
                                    propertyAttribute.TextForFalseValue);
                            }
                            else
                            {
                                tableRow.AddCell(
                                    propertyAttribute.Order,
                                    property.GetValue(entity),
                                    propertyAttribute.Type,
                                    propertyAttribute.Editable,
                                    property.Name,
                                    property.PropertyType.IsEnum ? property.PropertyType : null);
                            }
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
                                string propertyValue = entity.GetType().GetProperty(propertyName)?.GetValue(entity).ToString();
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

        public static TableRowActionViewModel CreateAction(string title, string icon, Color color, TableRowActionMethod method, string urlStringFormat, bool isToBeOpenedInNewTab, params object[] parameters)
        {
            return new TableRowActionViewModel(title, icon, color, urlStringFormat, parameters.Select(x => x.ToString()).ToList(), method, isToBeOpenedInNewTab);
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

        public static TableRowActionViewModel RestoreAction(string urlStringFormat, params object[] parameters)
        {
            var action = CreateAction("Restore", MaterialDesignIcons.Restore, Color.LimeGreen, TableRowActionMethod.Post, urlStringFormat, parameters);
            action.SetConfirmation("Restore Entity", "Are you sure you want to restore this deleted entity?");

            return action;
        }

        private static void SortTableCellAttributesAndPropertyNamesByOrderWeight(
            IList<TableCellAttribute> dtoAttributes,
            IList<string> tableCellAttributePropertyNames,
            IList<SortablePropertyAttribute> orderPropertyAttributes)
        {
            for (int i = 0; i < dtoAttributes.Count; i++)
            {
                for (int j = 0; j < dtoAttributes.Count - i - 1; j++)
                {
                    if (dtoAttributes[j].Order > dtoAttributes[j + 1].Order)
                    {
                        var tempTableCellAttribute = dtoAttributes[j];
                        dtoAttributes[j] = dtoAttributes[j + 1];
                        dtoAttributes[j + 1] = tempTableCellAttribute;

                        var tempPropertyName = tableCellAttributePropertyNames[j];
                        tableCellAttributePropertyNames[j] = tableCellAttributePropertyNames[j + 1];
                        tableCellAttributePropertyNames[j + 1] = tempPropertyName;

                        var tempOrderPropertyAttribute = orderPropertyAttributes[j];
                        orderPropertyAttributes[j] = orderPropertyAttributes[j + 1];
                        orderPropertyAttributes[j + 1] = tempOrderPropertyAttribute;
                    }
                }
            }
        }
    }
}