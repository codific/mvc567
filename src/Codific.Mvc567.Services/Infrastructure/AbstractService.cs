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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Codific.Mvc567.Entities.Database;

namespace Codific.Mvc567.Services.Infrastructure
{
    public abstract class AbstractService
    {
        protected const int PaginationPageSize = 10;

        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IStandardRepository standardRepository;

        public AbstractService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.standardRepository = this.uow.GetStandardRepository();
        }

        public IUnitOfWork Uow => this.uow;

        public IMapper Mapper => this.mapper;

        public IStandardRepository StandardRepository => this.standardRepository;

        protected async Task LogErrorAsync(Exception exception, string method)
        {
            try
            {
                string serviceClass = this.GetType().Name;
                Log log = new Log
                {
                    StackTrace = exception.StackTrace,
                    Source = exception.Source,
                    Message = exception.Message,
                    Method = method,
                    Class = serviceClass,
                };

                this.standardRepository.Add(log);
                await this.uow.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

        protected void LogError(Exception exception, string method)
        {
            try
            {
                string serviceClass = this.GetType().Name;
                Log log = new Log
                {
                    StackTrace = exception.StackTrace,
                    Source = exception.Source,
                    Message = exception.Message,
                    Method = method,
                    Class = serviceClass,
                };

                this.standardRepository.Add(log);
                this.uow.SaveChanges();
            }
            catch (Exception)
            {
            }
        }

        protected virtual Expression<Func<TEntity, bool>> GetEntitySearchQueryExpression<TEntity>(string searchQuery, bool showDeleted = false)
            where TEntity : class, IEntityBase
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

            var propertyExpression = expressionsList.FirstOrDefault();
            for (int i = 1; i < expressionsList.Count; i++)
            {
                propertyExpression = ExpressionFunctions.OrElse<TEntity>(propertyExpression, expressionsList[i]);
            }

            var resultExpression = ExpressionFunctions.AndAlso<TEntity>(x => x.Deleted == showDeleted, propertyExpression);

            return resultExpression;
        }
    }
}