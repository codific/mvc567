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

using Microsoft.Extensions.DependencyInjection;
using Codific.Mvc567.DataAccess.Abstraction.Context;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Infrastructure;

namespace Codific.Mvc567.Services.Extensions
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices<TDatabaseContext>(this IServiceCollection services) where TDatabaseContext : DatabaseContextBase<TDatabaseContext, User, Role>
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEntityManager, EntityManager>();
            services.AddScoped<ISEOService, SEOService>();
            services.AddScoped<IFileSystemService, FileSystemService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IStaticPageService, StaticPageService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILogService, LogService>();
            services.AddSingleton<ISingletonSecurityService, SingletonSecurityService>();

            return services;
        }
    }
}
