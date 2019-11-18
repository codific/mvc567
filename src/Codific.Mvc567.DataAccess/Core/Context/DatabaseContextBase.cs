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
using System.Threading;
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstractions.Context;
using Codific.Mvc567.Entities.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Codific.Mvc567.DataAccess.Core.Context
{
    public abstract class DatabaseContextBase<TContext, TUser, TRole> : IdentityDbContext<TUser, TRole, Guid>, IDatabaseContext
        where TContext : IdentityDbContext<TUser, TRole, Guid>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        public DatabaseContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
        }

        public Guid? CurrentUserId { get; set; }

        public override int SaveChanges()
        {
            this.MapEntitiesToUser();
            this.UpdateAuditableEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MapEntitiesToUser();
            this.UpdateAuditableEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MapEntitiesToUser();
            this.UpdateAuditableEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MapEntitiesToUser();
            this.UpdateAuditableEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry> modifiedEntityEntries = this.ChangeTracker
                .Entries()
                .Where(x => x.Entity is AuditableEntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entry in modifiedEntityEntries)
            {
                var entity = (AuditableEntityBase)entry.Entity;
                DateTime now = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = now;
                    entity.CreatedBy = this.CurrentUserId?.ToString();
                }
                else
                {
                    this.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                    this.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                }

                entity.UpdatedOn = now;
                entity.UpdatedBy = this.CurrentUserId?.ToString();
            }

            IEnumerable<EntityEntry> softDeletedEntityEntries = this.ChangeTracker
                .Entries()
                .Where(x => x.Entity is EntityBase && (x.State == EntityState.Modified));

            foreach (EntityEntry entry in softDeletedEntityEntries)
            {
                var entity = (EntityBase)entry.Entity;
                if (entry.State == EntityState.Modified && entity.Deleted && entity.DeletedBy is null)
                {
                    entity.DeletedBy = this.CurrentUserId?.ToString();
                }
                else if (entry.State == EntityState.Modified && !entity.Deleted && entity.DeletedBy != null)
                {
                    entity.DeletedBy = null;
                }
            }
        }

        protected virtual void MapEntitiesToUser()
        {
            if (this.CurrentUserId == null)
            {
                return;
            }

            IEnumerable<EntityEntry> modifiedEntityEntries = this.ChangeTracker
                .Entries()
                .Where(x => x.Entity is AuditableByUserEntityBase<TUser> && (x.State == EntityState.Added));

            foreach (EntityEntry entry in modifiedEntityEntries)
            {
                var entity = (AuditableByUserEntityBase<TUser>)entry.Entity;
                entity.UserId = this.CurrentUserId.Value;
            }
        }
    }
}