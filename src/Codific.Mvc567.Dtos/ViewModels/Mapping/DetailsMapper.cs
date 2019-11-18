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
using System.Linq;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Details;

namespace Codific.Mvc567.Entities.ViewModels.Mapping
{
    public static class DetailsMapper
    {
        public static DetailsViewModel DtoMapper<T>(T entity)
        {
            Type dtoType = typeof(T);
            DetailsViewModel detailsViewModel = new DetailsViewModel();
            var properties = dtoType.GetProperties();
            foreach (var property in properties)
            {
                if (property.HasAttribute<DetailsOrderAttribute>())
                {
                    var attribute = property.GetAttribute<DetailsOrderAttribute>();
                    int order = attribute.Order;
                    DetailsPropertyViewModel detailsProperty = new DetailsPropertyViewModel();
                    detailsProperty.Name = string.IsNullOrEmpty(attribute.Title) ? StringFunctions.SplitWordsByCapitalLetters(property.Name) : attribute.Title;
                    detailsProperty.Value = property.GetValue(entity);
                    detailsProperty.Type = TableCellType.Text;
                    if (property.GetCustomAttributes(typeof(TableCellAttribute), false).Length > 0)
                    {
                        detailsProperty.Type = ((TableCellAttribute)property.GetCustomAttributes(typeof(TableCellAttribute), false).FirstOrDefault()).Type;
                    }

                    detailsViewModel.AddProperty(detailsProperty, order);
                }
            }

            return detailsViewModel;
        }
    }
}