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

using Codific.Mvc567.DataAccess.Abstraction.Context;
using Codific.Mvc567.DataAccess.Abstraction.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codific.Mvc567.DataAccess.Abstraction
{
    public abstract class UnitOfWork<TContext, TUser, TRole> : IUnitOfWork
        where TContext : DatabaseContextBase<TContext, TUser, TRole>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        protected TContext context;
        protected readonly IServiceProvider serviceProvider;

        protected bool isDisposed;

        protected internal UnitOfWork(TContext context, IServiceProvider serviceProvider, IHttpContextAccessor httpAccessor)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;

            if (httpAccessor.HttpContext != null)
            {
                string userId = httpAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
                if (!string.IsNullOrEmpty(userId))
                {
                    Guid parsedUserId;
                    if (Guid.TryParse(userId, out parsedUserId))
                    {
                        this.context.CurrentUserId = parsedUserId;
                    }
                }
            }
        }

        public Dictionary<string, Type> DatabaseTables
        {
            get
            {
                return typeof(TContext)
                           .GetProperties()
                           .Where(x => 
                               x.PropertyType.AssemblyQualifiedName.Contains("Microsoft.EntityFrameworkCore.DbSet") || 
                               x.PropertyType.AssemblyQualifiedName.Contains("System.Collections.Generic.List"))
                           .ToDictionary(x => x.Name, x => x.PropertyType.GetTypeInfo().GenericTypeArguments[0]);
            }
        }

        public Guid? CurrentUserId
        {
            get
            {
                return this.context.CurrentUserId;
            }
        }

        public int SaveChanges()
        {
            CheckDisposed();
            return this.context.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CheckDisposed();
            return this.context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public Task<int> SaveChangesAsync()
        {
            CheckDisposed();
            return this.context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            CheckDisposed();
            return this.context.SaveChangesAsync(cancellationToken);
        }

        public IStandardRepository GetStandardRepository()
        {
            CheckDisposed();
            var repositoryType = typeof(IStandardRepository);
            var repository = (IStandardRepository)this.serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new ArgumentException(String.Format("Repository {0} has not been found in the IoC container.", repositoryType.Name));
            }

            ((IRepositoryInjection)repository).SetContext(this.context);
            return repository;
        }

        public IRepository<TEntity> GetRepository<TEntity>()
        {
            CheckDisposed();
            var repositoryType = typeof(IRepository<TEntity>);
            var repository = (IRepository<TEntity>)this.serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new ArgumentException(String.Format("Repository {0} has not been found in the IoC container.", repositoryType.Name));
            }

            ((IRepositoryInjection)repository).SetContext(this.context);
            return repository;
        }

        public TRepository GetCustomRepository<TRepository>()
        {
            CheckDisposed();
            var repositoryType = typeof(TRepository);
            var repository = (TRepository)this.serviceProvider.GetService(repositoryType);
            if (repository == null)
            {
                throw new ArgumentException(String.Format("Repository {0} has not been found in the IoC container.", repositoryType.Name));
            }

            ((IRepositoryInjection)repository).SetContext(this.context);
            return repository;
        }

        protected void CheckDisposed()
        {
            if (this.isDisposed) throw new ObjectDisposedException("The UOW is already disposed.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    if (this.context != null)
                    {
                        this.context.Dispose();
                        this.context = null;
                    }
                }
            }
            this.isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
