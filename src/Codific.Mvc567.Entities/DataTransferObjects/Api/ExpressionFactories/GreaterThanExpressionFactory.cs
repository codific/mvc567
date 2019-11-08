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

using Codific.Mvc567.Common.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Codific.Mvc567.Entities.DataTransferObjects.Api.ExpressionFactories
{
    public class GreaterThanExpressionFactory : AbstractExpressionFactory
    {
        public override Expression<Func<TEntity, bool>> BuildExpressionByQueryStringItem<TEntity>(FilterSearchQueryItem queryStringItem)
        {
            Type entityType = typeof(TEntity);
            PropertyInfo propertyInfo = entityType.GetProperty(queryStringItem.PropertyName);

            Expression<Func<TEntity, bool>> expression = null;

            if (propertyInfo.PropertyType.IsNumericType())
            {
                expression = x => propertyInfo.GetValue(x) != null && Convert.ToDecimal(propertyInfo.GetValue(x)) > Convert.ToDecimal(queryStringItem.Value);
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                expression = x => propertyInfo.GetValue(x) != null && Convert.ToDateTime(propertyInfo.GetValue(x)) > Convert.ToDateTime(queryStringItem.Value);
            }

            return expression;
        }
    }
}
