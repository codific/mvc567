using System;
using Codific.Mvc567.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Codific.Mvc567.Extensions
{
    public static class StartupApplicationBuilderExtensions
    {
        public static IRouteBuilder RegisterMvc567Routes(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "area-route",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            routeBuilder.MapRoute(
                name: "default-languages",
                template: Constants.LanguageControllerPageRoute + "/{controller=Home}/{action=Index}/{id?}");

            return routeBuilder;
        }

        public static IApplicationBuilder UseMvc567Middlewares(this IApplicationBuilder applicationBuilder, IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseBrowserLink();
                applicationBuilder.UseDeveloperExceptionPage();
                applicationBuilder.UseDatabaseErrorPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/error/400");
            }

            applicationBuilder.UseStatusCodePagesWithReExecute("/error/{0}");
            applicationBuilder.UseHealthChecks("/system/health");
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseAuthentication();

            return applicationBuilder;
        }
    }
}
