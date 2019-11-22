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
using Codific.Mvc567.DataAccess.Core.Context;
using Codific.Mvc567.Entities.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Codific.Mvc567.DataAccess
{
    public abstract class AbstractDatabaseContext<TContext> : DatabaseContextBase<TContext, User, Role>
        where TContext : IdentityDbContext<User, Role, Guid>
    {
        public AbstractDatabaseContext(DbContextOptions<TContext> options)
            : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<SystemConstant> SystemConstants { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<TranslationKey> TranslationKeys { get; set; }

        public DbSet<TranslationValue> TranslationValues { get; set; }

        public DbSet<AdminNavigationScheme> AdminNavigationSchemes { get; set; }

        public DbSet<SidebarMenuSectionItem> SidebarMenuSectionItems { get; set; }

        public DbSet<SidebarNavigationLinkItem> SidebarNavigationLinkItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Language>().HasIndex(x => x.Code).IsUnique();
            builder.Entity<Language>().HasOne(x => x.Image).WithOne().OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        }
    }
}
