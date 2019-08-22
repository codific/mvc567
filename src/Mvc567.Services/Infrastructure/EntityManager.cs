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
using Mvc567.DataAccess.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mvc567.DataAccess.Abstraction.Entities;
using Mvc567.Services.Abstractions;
using Mvc567.Entities.DataTransferObjects.ServiceResults;
using Mvc567.Entities.Database;
using Mvc567.Common.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;
using Mvc567.Common.Enums;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Mvc567.Services.Infrastructure
{
    public class EntityManager : AbstractService, IEntityManager
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public EntityManager(IUnitOfWork uow, IMapper mapper, IHostingEnvironment hostingEnvironment) : base(uow, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<PaginatedEntitiesResult<TEntityDto>> GetAllEntitiesPaginatedAsync<TEntity, TEntityDto>(int page, string searchQuery = null) where TEntity : class, IEntityBase, new()
        {
            PaginatedEntitiesResult<TEntityDto> result = new PaginatedEntitiesResult<TEntityDto>();
            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.Trim();
                }

                var standardRepository = this.uow.GetStandardRepository();
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    result.Count = (await standardRepository.GetAllAsync<TEntity>()).Count();
                }
                else
                {
                    result.Count = (await standardRepository.QueryAsync<TEntity>(GetEntitySearchQueryExpression<TEntity>(searchQuery))).Count();
                }
                result.CurrentPage = page;
                result.PageSize = PaginationPageSize;

                IEnumerable<TEntity> entities = null;
                var firstLevelIncludeQuery = GetFirstLevelIncludeQuery<TEntity>();
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    entities = await standardRepository.GetPageAsync<TEntity>(result.StartRow, PaginationPageSize, null, firstLevelIncludeQuery);
                }
                else
                {
                    entities = await standardRepository.QueryPageAsync<TEntity>(result.StartRow, PaginationPageSize, GetEntitySearchQueryExpression<TEntity>(searchQuery), null, firstLevelIncludeQuery);
                }
                var dtoEntities = this.mapper.Map<IEnumerable<TEntityDto>>(entities);
                result.Entities = dtoEntities;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetAllEntitiesPaginatedAsync));
            }

            return result;
        }

        public async Task<TEntityDto> GetEntityAsync<TEntity, TEntityDto>(Guid id) where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var firstLevelIncludeQuery = GetFirstLevelIncludeQuery<TEntity>();
                var entity = await standardRepository.GetAsync<TEntity>(id, firstLevelIncludeQuery);
                var dtoEntity = this.mapper.Map<TEntityDto>(entity);

                return dtoEntity;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetEntityAsync));
                return default(TEntityDto);
            }
        }

        public async Task<Guid?> CreateEntityAsync<TEntity, TEntityDto>(TEntityDto entity) where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var mappedEntity = this.mapper.Map<TEntity>(entity);

                await MoveTempFileAsync<TEntity>(mappedEntity);

                standardRepository.Add<TEntity>(mappedEntity);
                await this.uow.SaveChangesAsync();

                return mappedEntity.Id;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(CreateEntityAsync));
                return null;
            }
        }

        public async Task<Guid?> ModifyEntityAsync<TEntity, TEntityDto>(Guid id, TEntityDto modifiedEntity) where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var entityExist = (await standardRepository.CountAsync<TEntity>(x => x.Id == id)) != 0;
                if (entityExist)
                {
                    TEntity mappedEntity = this.mapper.Map<TEntity>(modifiedEntity);
                    await MoveTempFileAsync<TEntity>(mappedEntity);

                    mappedEntity.Id = id;
                    standardRepository.Update<TEntity>(mappedEntity);
                    await this.uow.SaveChangesAsync();

                    return id;
                }

                return null;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(ModifyEntityAsync));
                return null;
            }
        }

        public async Task<bool> DeleteEntityAsync<TEntity>(Guid id) where TEntity : class, IEntityBase, new()
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var entity = await standardRepository.GetAsync<TEntity>(id);
                standardRepository.Remove<TEntity>(entity);
                await this.uow.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(DeleteEntityAsync));
                return false;
            }
        }

        public async Task MoveTempFileAsync<TEntity>(TEntity entity)
        {
            try
            {
                var fileProperty = typeof(TEntity).GetProperties().Where(x => x.PropertyType == typeof(File)).FirstOrDefault();
                if (fileProperty != null)
                {
                    var saveDirectorySettings = fileProperty.GetSaveDirectorySettingsFromAttribute();
                    if (saveDirectorySettings.HasValue)
                    {
                        var fileIdPropertyName = fileProperty.GetAttribute<ForeignKeyAttribute>().Name;
                        var fileIdProperty = typeof(TEntity).GetProperty(fileIdPropertyName);
                        Guid fileId = Guid.Parse(fileIdProperty.GetValue(entity).ToString());
                        var standardRepository = this.uow.GetStandardRepository();
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
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(MoveTempFileAsync));
            }
        }

        private Func<IQueryable<TEntity>, IQueryable<TEntity>> GetFirstLevelIncludeQuery<TEntity>() where TEntity : class, IEntityBase, new()
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
    }
}

