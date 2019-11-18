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
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.Dtos.Api;
using Codific.Mvc567.Dtos.ServiceResults;

namespace Codific.Mvc567.Services.Abstractions
{
    public interface IEntityManager
    {
        Task<IEnumerable<TEntityDto>> GetAllEntitiesAsync<TEntity, TEntityDto>() where TEntity : class, IEntityBase, new();

        Task<TEntityDto> GetEntityAsync<TEntity, TEntityDto>(Guid id) where TEntity : class, IEntityBase, new();

        Task<PaginatedEntitiesResult<TEntityDto>> GetAllEntitiesPaginatedAsync<TEntity, TEntityDto>(int page, string searchQuery = null, bool showDeleted = false) where TEntity : class, IEntityBase, new();

        Task<Guid?> CreateEntityAsync<TEntity, TEntityDto>(TEntityDto entity) where TEntity : class, IEntityBase, new();

        Task<Guid?> ModifyEntityAsync<TEntity, TEntityDto>(Guid id, TEntityDto modifiedEntity) where TEntity : class, IEntityBase, new();

        Task<bool> DeleteEntityAsync<TEntity>(Guid id, bool softDelete = true) where TEntity : class, IEntityBase, new();
        
        Task<bool> RestoreEntityAsync<TEntity>(Guid id) where TEntity : class, IEntityBase, new();

        Task MoveTempFileAsync<TEntity>(TEntity entity);

        Task<PaginatedEntitiesResult<TEntityDto>> FilterEntitiesAsync<TEntity, TEntityDto>(FilterQueryRequest filterQuery) where TEntity : class, IEntityBase, new();

        Task<bool> ModifyEntityPropertyAsync<TEntity, TEntityDto>(Guid entityId, string property, string value) where TEntity : class, IEntityBase, new();
    }
}
