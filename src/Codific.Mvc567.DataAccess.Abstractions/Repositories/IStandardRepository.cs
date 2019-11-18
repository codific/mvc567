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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstractions.Entities;

namespace Codific.Mvc567.DataAccess.Abstractions.Repositories
{
    public interface IStandardRepository
    {
        IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false)
            where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false)
            where TEntity : class, IEntityBase, new();

        void Load<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        Task LoadAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        IEnumerable<TEntity> GetPage<TEntity>(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false)
            where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> GetPageAsync<TEntity>(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false)
            where TEntity : class, IEntityBase, new();

        TEntity Get<TEntity>(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        Task<TEntity> GetAsync<TEntity>(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        void Load<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        Task LoadAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        IEnumerable<TEntity> QueryPage<TEntity>(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> QueryPageAsync<TEntity>(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
            where TEntity : class, IEntityBase, new();

        void SetUnchanged<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        void Add<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        void Remove<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        void Remove<TEntity>(Guid id)
            where TEntity : class, IEntityBase, new();

        void SoftDelete<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        void SoftDelete<TEntity>(Guid id)
            where TEntity : class, IEntityBase, new();

        void Restore<TEntity>(TEntity entity)
            where TEntity : class, IEntityBase, new();

        void Restore<TEntity>(Guid id)
            where TEntity : class, IEntityBase, new();

        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntityBase, new();

        IEnumerable<IEntityBase> GetAllByEntityTableName(string entityTableName);

        IEnumerable<IEntityBase> GetAllByType(Type entityType);

        void DeleteAll<TEntity>()
            where TEntity : class, IEntityBase, new();

        Task DeleteAllAsync<TEntity>()
            where TEntity : class, IEntityBase, new();
    }
}