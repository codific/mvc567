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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mvc567.DataAccess.Abstraction;
using Mvc567.Common.Utilities;
using Mvc567.Common.Attributes;
using Mvc567.Entities.Database;
using Mvc567.DataAccess.Abstraction.Repositories;

namespace Mvc567.Services.Abstractions
{
    public abstract class AbstractService : IAbstractService
    {
        protected readonly IUnitOfWork uow;
        protected readonly IMapper mapper;
        protected readonly IStandardRepository standardRepository;

        protected const int PaginationPageSize = 10;

        public AbstractService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.standardRepository = this.uow.GetStandardRepository();
        }

        protected async Task LogErrorAsync(Exception exception, string method)
        {
            try
            {
                string serviceClass = this.GetType().Name;
                var standardRepository = this.uow.GetStandardRepository();
                Log log = new Log
                {
                    StackTrace = exception.StackTrace,
                    Source = exception.Source,
                    Message = exception.Message,
                    Method = method,
                    Class = serviceClass
                };

                standardRepository.Add(log);
                await this.uow.SaveChangesAsync();
            }
            catch (Exception) { }
        }

        protected void LogError(Exception exception, string method)
        {
            try
            {
                string serviceClass = this.GetType().Name;
                var standardRepository = this.uow.GetStandardRepository();
                Log log = new Log
                {
                    StackTrace = exception.StackTrace,
                    Source = exception.Source,
                    Message = exception.Message,
                    Method = method,
                    Class = serviceClass
                };

                standardRepository.Add(log);
                this.uow.SaveChanges();
            }
            catch (Exception) { }
        }

        protected virtual Expression<Func<TEntity, bool>> GetEntitySearchQueryExpression<TEntity>(string searchQuery)
        {
            var searchQueryArray = searchQuery.ToLower().Split(' ', ',', ';');

            Type entityType = typeof(TEntity);
            var entityProperties = entityType.GetProperties();
            var expressionsList = new List<Expression<Func<TEntity, bool>>>();
            foreach (var property in entityProperties)
            {
                if (property.GetCustomAttributes(typeof(SearchCriteriaAttribute), false).Length > 0)
                {
                    Expression<Func<TEntity, bool>> currentExpression = x =>
                           x.GetType().GetProperty(property.Name).GetValue(x) != null &&
                           (x.GetType().GetProperty(property.Name).GetValue(x).ToString().ToLower().Contains(searchQuery.ToLower()) ||
                            searchQueryArray.Contains(x.GetType().GetProperty(property.Name).GetValue(x).ToString().ToLower()));

                    expressionsList.Add(currentExpression);
                }
            }

            var resultExpression = expressionsList.FirstOrDefault();
            for (int i = 1; i < expressionsList.Count; i++)
            {
                resultExpression = ExpressionFunctions.OrElse<TEntity>(resultExpression, expressionsList[i]);
            }

            return resultExpression;
        }
    }
}
