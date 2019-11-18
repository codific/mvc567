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
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codific.Mvc567.DataAccess.Core.Repositories
{
    public abstract class Repository<TContext, TEntity> : RepositoryBase<TContext>, IRepository<TEntity>
        where TContext : DbContext
        where TEntity : class, IEntityBase, new()
    {
        private readonly Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> defaultOrderBy;

        protected Repository(TContext context)
            : base(context)
        {
            this.defaultOrderBy = new Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>(x => { return x.AsQueryable().OrderByDescending(y => y.Id); });
        }

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(null, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(null, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> GetPage(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null)
            {
                orderBy = this.defaultOrderBy;
            }

            var result = this.QueryDb(null, orderBy, includes, startRow, pageLength);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null)
            {
                orderBy = this.defaultOrderBy;
            }

            var result = this.QueryDb(null, orderBy, includes, startRow, pageLength);
            return await result.ToListAsync();
        }

        public virtual TEntity Get(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> query = this.Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return query.SingleOrDefault(x => x.Id == id);
        }

        public virtual async Task<TEntity> GetAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> query = this.Context.Set<TEntity>();

            if (includes != null)
            {
                query = includes(query);
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            var result = this.QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<TEntity> QueryPage(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null)
            {
                orderBy = this.defaultOrderBy;
            }

            var result = this.QueryDb(filter, orderBy, includes, startRow, pageLength);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPageAsync(int startRow, int pageLength, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            if (orderBy == null)
            {
                orderBy = this.defaultOrderBy;
            }

            var result = this.QueryDb(filter, orderBy, includes, startRow, pageLength);
            return await result.ToListAsync();
        }

        public void SetUnchanged(TEntity entity)
        {
            this.Context.Entry<TEntity>(entity).State = EntityState.Unchanged;
        }

        public void Add(TEntity entity)
        {
            this.Context.Set<TEntity>().Add(entity);
        }

        public TEntity Update(TEntity entity)
        {
            return this.Context.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void Remove(TEntity entity)
        {
            this.SetUnchanged(entity);
            this.Context.Set<TEntity>().Attach(entity);
            this.Context.Entry(entity).State = EntityState.Deleted;
            this.Context.Set<TEntity>().Remove(entity);
        }

        public virtual void Remove(Guid id)
        {
            var entity = new TEntity() { Id = id };
            this.Remove(entity);
        }

        protected IQueryable<TEntity> QueryDb(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes, int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = this.Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (take != null && skip != null)
            {
                query = query.Skip(skip.Value).Take(take.Value);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}