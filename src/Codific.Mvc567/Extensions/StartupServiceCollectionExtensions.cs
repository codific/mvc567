using System;
using System.Text;
using AutoMapper;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Options;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Codific.Mvc567.Extensions
{
    public static class StartupServiceCollectionExtensions
    {
        public static IServiceCollection AddMvc567Database<TDatabaseContext, TStandardRepository>(this IServiceCollection services, IConfiguration configuration, string migrationAssembly)
            where TDatabaseContext : AbstractDatabaseContext<TDatabaseContext>
            where TStandardRepository : class, IStandardRepository
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<TDatabaseContext>(options => { options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationAssembly)); })
                .BuildServiceProvider();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<TDatabaseContext>()
                .AddDefaultTokenProviders();

            services.RegisterDataAccessProviders<ApplicationUnitOfWork<TDatabaseContext>, TDatabaseContext, User, Role>();
            services.AddScoped<IStandardRepository, TStandardRepository>();

            return services;
        }

        public static IServiceCollection AddMvc567Services<TDatabaseContext>(this IServiceCollection services)
            where TDatabaseContext : AbstractDatabaseContext<TDatabaseContext>
        {
            services.RegisterValidationProvider();
            services.RegisterServices<TDatabaseContext>();
            services.AddScoped<IApplicationDatabaseInitializer, DatabaseInitializer<TDatabaseContext>>();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddMvc567Identity(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
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
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
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

                options.AddPolicy(Policies.AuthorizedUploadPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme);
                });
            });

            return services;
        }

        public static IServiceCollection AddMvc567FeatureProviders(this IServiceCollection services, ApplicationPartManager applicationPart = null)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .ConfigureApplicationPartManager(p =>
                {
                    if (applicationPart != null)
                    {
                        p = applicationPart;
                    }

                    p.ApplicationParts.Add(ApplicationAssemblyPart.AssemblyPart);
                    p.ApplicationParts.Add(UIAssemblyPart.AssemblyPart);
                    p.FeatureProviders.Add(new ViewComponentFeatureProvider());
                });

            return services;
        }

        public static IServiceCollection AddMvc567Mapping(this IServiceCollection services, string migrationAssembly, Action<IMapperConfigurationExpression> configurationAction)
        {
            IMapperConfigurationExpression customConfiguration = null;
            configurationAction.Invoke(customConfiguration);

            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(configuration =>
            {
                if (customConfiguration != null)
                {
                    configuration = customConfiguration;
                }

                configuration.AddMaps("Codific.Mvc567.Entities");
                configuration.AddMaps("Codific.Mvc567.ViewModels");
                configuration.AllowNullCollections = true;
                configuration.AllowNullDestinationValues = true;
                configuration.AddMaps(migrationAssembly);
                configuration.AddProfile<BaseMappingProfile>();
            })));

            return services;
        }

        public static IServiceCollection AddMvc567Configuration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleRecaptchaKeys>(configuration.GetSection("GoogleRecaptchaKeys"));
            services.AddScoped<InvisibleReCaptchaValidateAttribute>();
            services.AddScoped<VisibleReCaptchaValidateAttribute>();
            services.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));

            return services;
        }

        public static IServiceCollection AddMvc567Views(this IServiceCollection services)
        {
            services.ConfigureRazorViews();

            return services;
        }
    }
}
