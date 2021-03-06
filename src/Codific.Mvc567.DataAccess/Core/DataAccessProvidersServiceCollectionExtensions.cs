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
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Abstractions;
using Codific.Mvc567.DataAccess.Abstractions.Context;
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Codific.Mvc567.DataAccess.Core.Context;
using Codific.Mvc567.DataAccess.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Codific.Mvc567.DataAccess.Core
{
    public static class DataAccessProvidersServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataAccessProviders<TUnitOfWork, TContext, TUser, TRole>(this IServiceCollection services)
            where TUnitOfWork : UnitOfWork<TContext, TUser, TRole>
            where TContext : DatabaseContextBase<TContext, TUser, TRole>
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            services.AddTransient<IDatabaseContext, TContext>();
            services.AddTransient<IUnitOfWork, TUnitOfWork>();
            services.AddScoped<IStandardRepository, StandardRepository<TContext>>();

            return services;
        }
    }
}
