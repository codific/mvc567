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

using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Codific.Mvc567.DataAccess.Core.Repositories
{
    public class StandardRepository<TContext> : RepositoryBase<TContext>, IStandardRepository where TContext : DbContext
    {
        public StandardRepository(TContext context) : base(context)
        { }

        public virtual IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(x => x.Deleted == showDeleted, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(x => x.Deleted == showDeleted, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> GetPage<TEntity>(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false) where TEntity : class, IEntityBase, new()
        {
            if (orderBy == null) orderBy = new Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(x =>
            {
                return x.AsQueryable().OrderByDescending(y => y.Id);
            });

            var result = QueryDb(x => x.Deleted == showDeleted, orderBy, includes, startRow, pageLength);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync<TEntity>(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null, bool showDeleted = false) where TEntity : class, IEntityBase, new()
        {
            if (orderBy == null) orderBy = new Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(x =>
            {
                return x.AsQueryable().OrderByDescending(y => y.Id);
            });

            var result = QueryDb(x => x.Deleted == showDeleted, orderBy, includes, startRow, pageLength);
            return await result.ToListAsync();
        }

        public virtual TEntity Get<TEntity>(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return query.SingleOrDefault(x => x.Id == id);
        }

        public virtual async Task<TEntity> GetAsync<TEntity>(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> QueryPage<TEntity>(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            if (orderBy == null) orderBy = new Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(x =>
            {
                return x.AsQueryable().OrderByDescending(y => y.Id);
            });

            var result = QueryDb(filter, orderBy, includes, startRow, pageLength);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPageAsync<TEntity>(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null) where TEntity : class, IEntityBase, new()
        {
            if (orderBy == null) orderBy = new Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(x =>
            {
                return x.AsQueryable().OrderByDescending(y => y.Id);
            });

            var result = QueryDb(filter, orderBy, includes, startRow, pageLength);
            return await result.ToListAsync();
        }

        protected IQueryable<TEntity> QueryDb<TEntity>(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (take != null && skip != null)
            {
                query = query.Skip(skip.Value).Take(take.Value);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            return query;
        }

        public void SetUnchanged<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            base.Context.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            Context.Set<TEntity>().Add(entity);
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            return Context.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void Remove<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            SetUnchanged<TEntity>(entity);
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void Remove<TEntity>(Guid id) where TEntity : class, IEntityBase, new()
        {
            var entity = new TEntity() { Id = id };
            this.Remove(entity);
        }

        public virtual void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;
        }

        public virtual void SoftDelete<TEntity>(Guid id) where TEntity : class, IEntityBase, new()
        {
            var entity = new TEntity() { Id = id };
            entity.Deleted = true;
            entity.DeletedOn = DateTime.Now;
        }

        public virtual void Restore<TEntity>(TEntity entity) where TEntity : class, IEntityBase, new()
        {
            entity.Deleted = false;
            entity.DeletedOn = null;
        }

        public virtual void Restore<TEntity>(Guid id) where TEntity : class, IEntityBase, new()
        {
            var entity = new TEntity() { Id = id };
            entity.Deleted = false;
            entity.DeletedOn = null;
        }

        public IEnumerable<IEntityBase> GetAllByEntityTableName(string entityTableName)
        {
            var entities = this.Context.GetType().GetProperties().Where(x => x.Name == entityTableName).FirstOrDefault()?.GetValue(this.Context);
            IEnumerable<IEntityBase> resultEntities = null;
            if (entities != null)
            {
                resultEntities = ((IEnumerable<IEntityBase>)entities).ToList();
            }

            return resultEntities;
        }

        public IEnumerable<IEntityBase> GetAllByType(Type entityType)
        {
            var entities = this.Context.GetType().GetProperties().Where(x => x.PropertyType.GenericTypeArguments.Contains(entityType)).FirstOrDefault()?.GetValue(this.Context); ;
            IEnumerable<IEntityBase> resultEntities = null;
            if (entities != null)
            {
                resultEntities = ((IEnumerable<IEntityBase>)entities).ToList();
            }

            return resultEntities;
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class, IEntityBase, new()
        {
            var result = QueryDb(filter, null, null);
            return await result.CountAsync();
        }

        public void DeleteAll<TEntity>() where TEntity : class, IEntityBase, new()
        {
            Context.RemoveRange(GetAll<TEntity>());
        }

        public async Task DeleteAllAsync<TEntity>() where TEntity : class, IEntityBase, new()
        {
            Context.RemoveRange(await GetAllAsync<TEntity>());
        }
    }
}
