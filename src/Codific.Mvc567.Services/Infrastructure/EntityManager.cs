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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.CommonCore;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.Dtos.Api;
using Codific.Mvc567.Dtos.Api.ExpressionFactories;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using PhoneAttribute = System.ComponentModel.DataAnnotations.PhoneAttribute;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class EntityManager : Service, IEntityManager
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public EntityManager(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment hostingEnvironment)
            : base(uow, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<TEntityDto>> GetAllEntitiesAsync<TEntity, TEntityDto>()
            where TEntity : class, IEntityBase, new()
        {
            List<TEntityDto> resultList = new List<TEntityDto>();
            try
            {
                var entities = await this.StandardRepository.GetAllAsync<TEntity>();
                if (entities != null && entities.Count() > 0)
                {
                    resultList = this.Mapper.Map<IEnumerable<TEntityDto>>(entities).ToList();
                }
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetAllEntitiesAsync));
            }

            return resultList;
        }

        public async Task<PaginatedEntitiesResult<TEntityDto>> GetAllEntitiesPaginatedAsync<TEntity, TEntityDto>(
            int page,
            string searchQuery = null,
            string sortProperty = null)
            where TEntity : class, IEntityBase, new()
        {
            PaginatedEntitiesResult<TEntityDto> result = new PaginatedEntitiesResult<TEntityDto>();
            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.Trim();
                }

                var standardRepository = this.Uow.GetStandardRepository();
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    result.Count = await standardRepository.CountAsync<TEntity>(null);
                }
                else
                {
                    var searchQueryExpression = this.GetEntitySearchQueryExpression<TEntity>(searchQuery);
                    result.Count = standardRepository.EnumerableCount<TEntity>(searchQueryExpression.Compile());
                }

                result.CurrentPage = page;
                result.PageSize = this.PaginationPageSize;

                var orderByExpression = this.GetOrderExpressionByFilterQueryRequest<TEntity, TEntityDto>(sortProperty);
                IEnumerable<TEntity> entities = null;
                var firstLevelIncludeQuery = this.GetFirstLevelIncludeQuery<TEntity>();
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    entities = await standardRepository.GetPageAsync<TEntity>(result.StartRow, this.PaginationPageSize, orderByExpression, firstLevelIncludeQuery);
                }
                else
                {
                    entities = await standardRepository.QueryPageAsync<TEntity>(result.StartRow, this.PaginationPageSize, this.GetEntitySearchQueryExpression<TEntity>(searchQuery), orderByExpression, firstLevelIncludeQuery);
                }

                var dtoEntities = this.Mapper.Map<IEnumerable<TEntityDto>>(entities);
                result.Entities = dtoEntities;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetAllEntitiesPaginatedAsync));
            }

            return result;
        }

        public async Task<TEntityDto> GetEntityAsync<TEntity, TEntityDto>(Guid id)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.Uow.GetStandardRepository();
                var firstLevelIncludeQuery = this.GetFirstLevelIncludeQuery<TEntity>();
                var entity = await standardRepository.GetAsync<TEntity>(id, firstLevelIncludeQuery);
                var dtoEntity = this.Mapper.Map<TEntityDto>(entity);

                return dtoEntity;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.GetEntityAsync));
                return default(TEntityDto);
            }
        }

        public async Task<Guid?> CreateEntityAsync<TEntity, TEntityDto>(TEntityDto entity)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.Uow.GetStandardRepository();
                var mappedEntity = this.Mapper.Map<TEntity>(entity);

                await this.MoveTempFileAsync<TEntity>(mappedEntity);

                standardRepository.Add<TEntity>(mappedEntity);
                await this.Uow.SaveChangesAsync();

                return mappedEntity.Id;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.CreateEntityAsync));
                return null;
            }
        }

        public async Task<Guid?> ModifyEntityAsync<TEntity, TEntityDto>(Guid id, TEntityDto modifiedEntity)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.Uow.GetStandardRepository();
                var entityExist = (await standardRepository.CountAsync<TEntity>(x => x.Id == id)) != 0;
                if (entityExist)
                {
                    TEntity mappedEntity = this.Mapper.Map<TEntity>(modifiedEntity);
                    await this.MoveTempFileAsync<TEntity>(mappedEntity);

                    mappedEntity.Id = id;
                    standardRepository.Update<TEntity>(mappedEntity);
                    await this.Uow.SaveChangesAsync();

                    return id;
                }

                return null;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.ModifyEntityAsync));
                return null;
            }
        }

        public async Task<bool> DeleteEntityAsync<TEntity>(Guid id, bool softDelete = true)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.Uow.GetStandardRepository();
                var entity = await standardRepository.GetAsync<TEntity>(id);
                if (softDelete)
                {
                    standardRepository.SoftDelete<TEntity>(entity);
                }
                else
                {
                    standardRepository.Remove<TEntity>(entity);
                }

                await this.Uow.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.DeleteEntityAsync));
                return false;
            }
        }

        public async Task<bool> RestoreEntityAsync<TEntity>(Guid id)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.Uow.GetStandardRepository();
                var entity = await standardRepository.GetAsync<TEntity>(id);
                standardRepository.Restore<TEntity>(entity);
                await this.Uow.SaveChangesAsync();

                return false;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.RestoreEntityAsync));
                return false;
            }
        }

        public async Task MoveTempFileAsync<TEntity>(TEntity entity)
        {
            try
            {
                var fileProperty = typeof(TEntity).GetProperties().FirstOrDefault(x => x.PropertyType == typeof(File));
                if (fileProperty != null)
                {
                    var saveDirectorySettings = fileProperty.GetSaveDirectorySettingsFromAttribute();
                    if (saveDirectorySettings.HasValue)
                    {
                        var fileIdPropertyName = fileProperty.GetAttribute<ForeignKeyAttribute>().Name;
                        var fileIdProperty = typeof(TEntity).GetProperty(fileIdPropertyName);
                        if (fileIdProperty != null)
                        {
                            Guid fileId = Guid.Parse(fileIdProperty.GetValue(entity).ToString());
                            var standardRepository = this.Uow.GetStandardRepository();
                            var fileEntity = await standardRepository.GetAsync<File>(fileId);
                            if (fileEntity != null && fileEntity.Temp)
                            {
                                string fileNameWithExtension = $"{fileEntity.Name}.{fileEntity.FileExtension.ToString().ToLower().Replace("_", string.Empty)}";
                                string oldFilePath = System.IO.Path.Combine(this.hostingEnvironment.ContentRootPath, fileEntity.Path);

                                fileEntity.Root = saveDirectorySettings.Value.Root;
                                fileEntity.Temp = false;
                                fileEntity.Path = System.IO.Path.Combine(saveDirectorySettings.Value.RelativePath, fileNameWithExtension);
                                fileEntity.RelativeUrl = fileEntity.Path.Replace('\\', '/');
                                if (saveDirectorySettings.Value.UserSpecific)
                                {
                                    fileEntity.Path = string.Format(saveDirectorySettings.Value.RelativePath, fileEntity.UserId);
                                }

                                string newFilePath = this.hostingEnvironment.WebRootPath;
                                if (fileEntity.Root == ApplicationRoots.Private)
                                {
                                    newFilePath = this.hostingEnvironment.GetPrivateRoot();
                                }

                                newFilePath = System.IO.Path.Combine(newFilePath, fileEntity.Path);

                                System.IO.File.Copy(oldFilePath, newFilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.MoveTempFileAsync));
            }
        }

        public async Task<PaginatedEntitiesResult<TEntityDto>> FilterEntitiesAsync<TEntity, TEntityDto>(FilterQueryRequest filterQuery)
            where TEntity : class, IEntityBase, new()
        {
            PaginatedEntitiesResult<TEntityDto> result = new PaginatedEntitiesResult<TEntityDto>();
            try
            {
                Expression<Func<TEntity, bool>> searchQueryExpression = null;
                if (filterQuery.FilterQueryStringItems != null && filterQuery.FilterQueryStringItems.Count > 0)
                {
                    searchQueryExpression = this.GetEntitySearchQueryExpressionByFilterQueryRequest<TEntity>(filterQuery);
                }
                else if (!string.IsNullOrEmpty(filterQuery.SearchQuery))
                {
                    searchQueryExpression = this.GetEntitySearchQueryExpression<TEntity>(filterQuery.SearchQuery);
                }

                Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> orderExpression = null; // this.GetOrderExpressionByFilterQueryRequest<TEntity>(filterQuery);
                var firstLevelIncludeQuery = this.GetFirstLevelIncludeQuery<TEntity>();

                if (searchQueryExpression != null)
                {
                    result.Count = this.StandardRepository.EnumerableCount<TEntity>(searchQueryExpression.Compile());
                }
                else
                {
                    result.Count = await this.StandardRepository.CountAsync<TEntity>(null);
                }

                result.CurrentPage = filterQuery.Page.HasValue ? filterQuery.Page.Value : 1;
                if (filterQuery.Page.HasValue)
                {
                    result.PageSize = filterQuery.PageSize.HasValue ? filterQuery.PageSize.Value : this.PaginationPageSize;
                }
                else
                {
                    result.PageSize = result.Count;
                }

                IEnumerable<TEntity> entities = null;

                if (searchQueryExpression != null)
                {
                    entities = this.StandardRepository.EnumerableQueryPage<TEntity>(result.StartRow, result.PageSize, searchQueryExpression.Compile(), orderExpression, firstLevelIncludeQuery);
                }
                else
                {
                    entities = this.StandardRepository.EnumerableQueryPage<TEntity>(result.StartRow, result.PageSize, null, orderExpression, firstLevelIncludeQuery);
                }

                result.Entities = this.Mapper.Map<IEnumerable<TEntityDto>>(entities);
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.FilterEntitiesAsync));
            }

            return result;
        }

        public async Task<InlineEditResult> ModifyEntityPropertyAsync<TEntity, TEntityDto>(Guid entityId, string property, string value)
            where TEntity : class, IEntityBase, new()
        {
            try
            {
                var dbEntity = await this.Uow.GetStandardRepository().GetAsync<TEntity>(entityId);
                var entity = this.Mapper.Map<TEntityDto>(dbEntity);
                var propertyForEdit = typeof(TEntityDto).GetProperty(property);
                var propertyAttributes = propertyForEdit.GetCustomAttributes(false);
                var entityCanBeModified = propertyForEdit != null && propertyForEdit.HasAttribute<CreateEditEntityInputAttribute>();

                if (entity != null && entityCanBeModified && !string.IsNullOrEmpty(value))
                {
                    object parsedValue = value?.ToString();
                    if (propertyForEdit.PropertyType == typeof(DateTime) || (propertyForEdit.PropertyType == typeof(DateTime?) && !string.IsNullOrEmpty(value)))
                    {
                        parsedValue = DateTime.ParseExact(value, Constants.DateFormat, CultureInfo.InvariantCulture);
                    }
                    else if (propertyForEdit.PropertyType == typeof(int))
                    {
                        parsedValue = int.Parse(value);
                    }
                    else if (propertyForEdit.PropertyType == typeof(double))
                    {
                        parsedValue = double.Parse(value);
                    }
                    else if (propertyForEdit.PropertyType.IsEnum)
                    {
                        parsedValue = int.Parse(value);
                    }
                    else if (propertyForEdit.PropertyType == typeof(bool))
                    {
                        if (value == "0")
                        {
                            parsedValue = false;
                        }
                        else if (value == "1")
                        {
                            parsedValue = true;
                        }
                    }

                    if (propertyAttributes.Any(x => x.GetType() == typeof(PhoneAttribute)))
                    {
                        var attribute = (PhoneAttribute)propertyAttributes.First(x => x.GetType() == typeof(PhoneAttribute));
                        if (!attribute.IsValid(value))
                        {
                            return new InlineEditResult(false, attribute.ErrorMessage);
                        }
                    }
                    else if (propertyAttributes.Any(x => x.GetType() == typeof(EmailAddressAttribute)))
                    {
                        var attribute = (EmailAddressAttribute)propertyAttributes.First(x => x.GetType() == typeof(EmailAddressAttribute));
                        if (!attribute.IsValid(value))
                        {
                            return new InlineEditResult(false, attribute.ErrorMessage);
                        }
                    }

                    propertyForEdit.SetValue(entity, parsedValue);
                    var editedId = await this.ModifyEntityAsync<TEntity, TEntityDto>(entityId, entity);
                    if (editedId.HasValue)
                    {
                        return new InlineEditResult(true);
                    }
                }

                return new InlineEditResult(false);
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.ModifyEntityPropertyAsync));
                return new InlineEditResult(false);
            }
        }

        protected virtual Expression<Func<TEntity, bool>> GetEntitySearchQueryExpressionByFilterQueryRequest<TEntity>(FilterQueryRequest filterQuery)
        {
            if (filterQuery.EmptyQuery)
            {
                return null;
            }

            var expressionsList = new List<Expression<Func<TEntity, bool>>>();
            if (filterQuery.FilterQueryStringItems != null && filterQuery.FilterQueryStringItems.Count > 0)
            {
                var expressionFactories = new ExpressionFactories();
                foreach (var queryStringItem in filterQuery.FilterQueryStringItems)
                {
                    Expression<Func<TEntity, bool>> currentExpression = expressionFactories[queryStringItem.EqualityType].BuildExpressionByQueryStringItem<TEntity>(queryStringItem);
                    if (currentExpression != null)
                    {
                        expressionsList.Add(currentExpression);
                    }
                }
            }

            var resultExpression = expressionsList.FirstOrDefault();
            for (int i = 1; i < expressionsList.Count; i++)
            {
                resultExpression = ExpressionFunctions.AndAlso(resultExpression, expressionsList[i]);
            }

            return resultExpression;
        }

        private Func<IQueryable<TEntity>, IQueryable<TEntity>> GetFirstLevelIncludeQuery<TEntity>()
            where TEntity : class, IEntityBase, new()
        {
            var targetProperties = typeof(TEntity).GetProperties().Where(x => x.HasAttribute<ForeignKeyAttribute>()).ToList();
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includeQuery = (x) =>
            {
                for (int i = 0; i < targetProperties.Count; i++)
                {
                    x = x.Include(targetProperties[i].Name);
                }

                return x;
            };

            return includeQuery;
        }

        private Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> GetOrderExpressionByFilterQueryRequest<TEntity>(FilterQueryRequest filterQuery)
        {
            if (string.IsNullOrEmpty(filterQuery.OrderBy) || filterQuery.FilterQueryOrderItems == null || filterQuery.FilterQueryOrderItems.Count == 0)
            {
                return null;
            }

            Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> function = (x) =>
            {
                IOrderedEnumerable<TEntity> mainOrderFunction = null;
                foreach (var orderItem in filterQuery.FilterQueryOrderItems)
                {
                    if (mainOrderFunction != null)
                    {
                        if (orderItem.OrderType == FilterOrderType.Ascending)
                        {
                            mainOrderFunction = mainOrderFunction.ThenBy(y => y.GetType().GetProperty(orderItem.PropertyName).GetValue(y));
                        }
                        else
                        {
                            mainOrderFunction = mainOrderFunction.ThenByDescending(y => y.GetType().GetProperty(orderItem.PropertyName).GetValue(y));
                        }
                    }
                    else
                    {
                        if (orderItem.OrderType == FilterOrderType.Ascending)
                        {
                            mainOrderFunction = x.OrderBy(y => y.GetType().GetProperty(orderItem.PropertyName).GetValue(y));
                        }
                        else
                        {
                            mainOrderFunction = x.OrderByDescending(y => y.GetType().GetProperty(orderItem.PropertyName).GetValue(y));
                        }
                    }
                }

                return mainOrderFunction;
            };

            return function;
        }
    }
}
