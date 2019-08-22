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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Mvc567.Common.Options;
using Mvc567.DataAccess.Abstraction;
using Mvc567.Services.Extensions;
using Mvc567.UI.Extensions;
using Mvc567.UI;
using Mvc567.DataAccess.Abstraction.Repositories;
using Microsoft.AspNetCore.Authorization;
using Mvc567.Common;
using Mvc567.Entities.Database;
using Mvc567.DataAccess.Identity;
using Mvc567.Common.Attributes;
using Mvc567.Middlewares;
using Mvc567.Seed;
using Mvc567.DataAccess;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;

namespace Mvc567
{
    public class ApplicationStartup<TDatabaseContext, TStandardRepository>
        where TDatabaseContext : AbstractDatabaseContext<TDatabaseContext>
        where TStandardRepository : class, IStandardRepository
    {
        protected string applicationAssembly = string.Empty;

        public ApplicationStartup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TDatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"), b => b.MigrationsAssembly(this.applicationAssembly));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<TDatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddHealthChecks();

            services.RegisterDataAccessProviders<ApplicationUnitOfWork<TDatabaseContext>, TDatabaseContext, User, Role>();
            services.AddScoped<IStandardRepository, TStandardRepository>();
            services.RegisterValidationProvider();
            services.RegisterServices<TDatabaseContext>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer<TDatabaseContext>>();

            services.ConfigureRazorViews();


            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(configuration =>
            {
                configuration.AddMaps("Mvc567.Entities");
                configuration.AllowNullCollections = true;
                configuration.AllowNullDestinationValues = true;
                RegisterMappingProfiles(ref configuration);
            })));

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/login";
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Mvc567 API", Version = "v1" });
            });

            RegisterServices(ref services);

            services.AddMvc()
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

        protected virtual void RegisterServices(ref IServiceCollection services)
        {

        }

        protected virtual void AddAuthorizationOptions(ref AuthorizationOptions options)
        {

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

            routes.MapRoute(
                name: "static-pages",
                template: Constants.ControllerStaticPageRoute,
                defaults: new { controller = "StaticPage", action = "PageAction" });

            routes.MapRoute(
                name: "static-pages-languages",
                template: Constants.LanguageControllerStaticPageRoute,
                defaults: new { controller = "StaticPage", action = "PageAction" });
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
            app.UseAdminRedirection();
            app.UseSwaggerAdminValidation();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            ConfigureMiddlewareAfterAuthentication(ref app);

            app.UseMvc(routes =>
            {
                RegisterRoutes(ref routes);
            });
        }
    }
}
