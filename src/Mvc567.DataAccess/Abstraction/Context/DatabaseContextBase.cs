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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mvc567.DataAccess.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvc567.DataAccess.Abstraction.Context
{
    public abstract class DatabaseContextBase<TContext, TUser, TRole> : IdentityDbContext<TUser, TRole, Guid>, IDatabaseContext
        where TContext : IdentityDbContext<TUser, TRole, Guid>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {

        public DatabaseContextBase(DbContextOptions<TContext> options) : base(options)
        {
        }

        public Guid? CurrentUserId { get; set; }

        public override int SaveChanges()
        {
            MapEntitiesToUser();
            UpdateAuditableEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            MapEntitiesToUser();
            UpdateAuditableEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            MapEntitiesToUser();
            UpdateAuditableEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            MapEntitiesToUser();
            UpdateAuditableEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void UpdateAuditableEntities()
        {
            IEnumerable<EntityEntry> modifiedEntityEntries = ChangeTracker
                .Entries()
                .Where(x => x.Entity is AuditableEntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entry in modifiedEntityEntries)
            {
                var entity = (AuditableEntityBase)entry.Entity;
                DateTime now = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = now;
                    entity.CreatedBy = CurrentUserId?.ToString();
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                }

                entity.UpdatedOn = now;
                entity.UpdatedBy = CurrentUserId?.ToString();
            }
        }

        protected virtual void MapEntitiesToUser()
        {
            if (CurrentUserId == null)
            {
                return;
            }

            IEnumerable<EntityEntry> modifiedEntityEntries = ChangeTracker
            .Entries()
            .Where(x => x.Entity is AuditableByUserEntityBase<TUser> && (x.State == EntityState.Added));

            foreach (EntityEntry entry in modifiedEntityEntries)
            {
                var entity = (AuditableByUserEntityBase<TUser>)entry.Entity;
                entity.UserId = CurrentUserId.Value;
            }
        }
    }
}
