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
using System.Text;
using AutoMapper;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Options;
using Codific.Mvc567.CommonCore;
using Codific.Mvc567.DataAccess;
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Codific.Mvc567.DataAccess.Core;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Profiles;
using Codific.Mvc567.Seed;
using Codific.Mvc567.Services.Extensions;
using Codific.Mvc567.UI;
using Codific.Mvc567.UI.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Codific.Mvc567
{
    public class ApplicationStartup<TDatabaseContext, TStandardRepository>
        where TDatabaseContext : AbstractDatabaseContext<TDatabaseContext>
        where TStandardRepository : class, IStandardRepository
    {
        protected string applicationAssembly = string.Empty;

        public ApplicationStartup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDbContext(ref services);

            services.AddHealthChecks();

            services.RegisterDataAccessProviders<ApplicationUnitOfWork<TDatabaseContext>, TDatabaseContext, User, Role>();
            services.AddScoped<IStandardRepository, TStandardRepository>();
            services.RegisterValidationProvider();
            services.RegisterServices<TDatabaseContext>();

            services.AddScoped<IApplicationDatabaseInitializer, DatabaseInitializer<TDatabaseContext>>();

            services.ConfigureRazorViews();


            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(configuration =>
            {
                configuration.AddMaps("Codific.Mvc567.Entities");
                configuration.AddMaps("Codific.Mvc567.ViewModels");
                configuration.AllowNullCollections = true;
                configuration.AllowNullDestinationValues = true;
                configuration.AddMaps(this.applicationAssembly);
                configuration.AddProfile<BaseMappingProfile>();
                RegisterMappingProfiles(ref configuration);
            })));

            services.Configure<IdentityOptions>(options =>
            {
                ConfigureIdentityOptions(ref options);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/admin/login";
                options.LogoutPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddAuthorization(options =>
            {
                foreach (ApplicationPermission permission in ApplicationPermissions.AllPermissions)
                {
                    options.AddPolicy(permission.Name, policy =>
                    {
                        policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(CustomClaimTypes.Permission, permission.Value);
                    });
                }

                AddAuthorizationOptions(ref options);
            });

            services.Configure<GoogleRecaptchaKeys>(Configuration.GetSection("GoogleRecaptchaKeys"));

            services.AddScoped<InvisibleReCaptchaValidateAttribute>();
            services.AddScoped<VisibleReCaptchaValidateAttribute>();

            services.Configure<SmtpConfig>(Configuration.GetSection("SmtpConfig"));

            RegisterServices(ref services);

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .ConfigureApplicationPartManager(p =>
                {
                    p.ApplicationParts.Add(ApplicationAssemblyPart.AssemblyPart);
                    p.ApplicationParts.Add(UIAssemblyPart.AssemblyPart);
                    p.FeatureProviders.Add(new ViewComponentFeatureProvider());
                    RegisterFeatureProviders(ref p);
                })
                .AddXmlSerializerFormatters();
            services.AddHttpContextAccessor();
        }

        protected virtual void RegisterDbContext(ref IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DatabaseConnection");

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<TDatabaseContext>(options =>
                {
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(this.applicationAssembly));
                })
                .BuildServiceProvider();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<TDatabaseContext>()
                .AddDefaultTokenProviders();
        }

        protected virtual void RegisterServices(ref IServiceCollection services)
        {

        }

        protected virtual void AddAuthorizationOptions(ref AuthorizationOptions options)
        {
            options.AddPolicy(Policies.AuthorizedUploadPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme);
            });
        }

        protected virtual void ConfigureMiddlewareBeforeAuthentication(ref IApplicationBuilder app)
        {

        }

        protected virtual void ConfigureMiddlewareAfterAuthentication(ref IApplicationBuilder app)
        {

        }

        protected virtual void RegisterMappingProfiles(ref IMapperConfigurationExpression configuration)
        {

        }

        protected virtual void RegisterFeatureProviders(ref ApplicationPartManager applicationPartManager)
        {

        }

        protected virtual void ConfigureIdentityOptions(ref IdentityOptions options)
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
        }

        protected virtual void RegisterRoutes(ref IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "area-route",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            routes.MapRoute(
                name: "default-languages",
                template: Constants.LanguageControllerPageRoute + "/{controller=Home}/{action=Index}/{id?}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error/400");
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            ConfigureMiddlewareBeforeAuthentication(ref app);

            app.UseHealthChecks("/system/health");
            app.UseStaticFiles();
            app.UseAuthentication();

            ConfigureMiddlewareAfterAuthentication(ref app);

            app.UseMvc(routes =>
            {
                RegisterRoutes(ref routes);
            });
        }
    }
}
