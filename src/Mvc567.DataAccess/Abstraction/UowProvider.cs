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

using Mvc567.DataAccess.Abstraction.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Mvc567.DataAccess.Abstraction
{
    public class UowProvider : IUowProvider
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public UowProvider()
        { }

        public UowProvider(ILogger logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork<TUnitOfWork, TContext, TUser, TRole>(bool trackChanges = true, bool enableLogging = false)
            where TUnitOfWork : UnitOfWork<TContext, TUser, TRole>, new()
            where TContext : DatabaseContextBase<TContext, TUser, TRole>
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            var context = (TContext)this.serviceProvider.GetService(typeof(IDatabaseContext));

            if (!trackChanges)
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            IUnitOfWork uow = Activator.CreateInstance(typeof(TUnitOfWork), new object[] { context, this.serviceProvider }) as IUnitOfWork;
            return uow;
        }
    }
}
