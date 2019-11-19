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

using Codific.Mvc567.Common;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Codific.Mvc567.UI.Extensions
{
    public static class ViewsServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRazorViews(
            this IServiceCollection services,
            string areaDefault = Constants.DefaultAreasViewsPath,
            string controllerDefault = Constants.DefaultControllersViewsPath,
            string emailDefault = Constants.DefaultEmailViewsPath,
            params string[] additionFeaturesPaths)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Views/Components/{0}.cshtml");
                options.ViewLocationFormats.Add($"{controllerDefault}/{{1}}/{{0}}.cshtml");
                options.ViewLocationFormats.Add($"{controllerDefault}/Shared/{{0}}.cshtml");
                if (!Constants.DefaultControllersViewsPath.Equals(controllerDefault))
                {
                    options.ViewLocationFormats.Add($"{Constants.DefaultControllersViewsPath}/{{1}}/{{0}}.cshtml");
                    options.ViewLocationFormats.Add($"{Constants.DefaultControllersViewsPath}/Shared/{{0}}.cshtml");
                }

                options.ViewLocationFormats.Add($"{emailDefault}/{{1}}/{{0}}.cshtml");
                options.ViewLocationFormats.Add($"{emailDefault}/Shared/{{0}}.cshtml");
                if (!Constants.DefaultEmailViewsPath.Equals(emailDefault))
                {
                    options.ViewLocationFormats.Add($"{Constants.DefaultEmailViewsPath}/{{1}}/{{0}}.cshtml");
                    options.ViewLocationFormats.Add($"{Constants.DefaultEmailViewsPath}/Shared/{{0}}.cshtml");
                }

                if (additionFeaturesPaths != null && additionFeaturesPaths.Length > 0)
                {
                    foreach (var path in additionFeaturesPaths)
                    {
                        options.ViewLocationFormats.Add($"{path}/{{1}}/{{0}}.cshtml");
                        options.ViewLocationFormats.Add($"{path}/Shared/{{0}}.cshtml");
                    }
                }

                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Views/Components/{0}.cshtml");
                options.AreaViewLocationFormats.Add($"{areaDefault}/{{2}}/{{1}}/{{0}}.cshtml");
                options.AreaViewLocationFormats.Add($"{areaDefault}/{{2}}/Shared/{{0}}.cshtml");
                options.AreaViewLocationFormats.Add($"{controllerDefault}/Shared/{{0}}.cshtml");
                if (!Constants.DefaultAreasViewsPath.Equals(areaDefault))
                {
                    options.AreaViewLocationFormats.Add($"{Constants.DefaultAreasViewsPath}/{{2}}/{{1}}/{{0}}.cshtml");
                    options.AreaViewLocationFormats.Add($"{Constants.DefaultAreasViewsPath}/{{2}}/Shared/{{0}}.cshtml");
                }

                if (!Constants.DefaultControllersViewsPath.Equals(controllerDefault))
                {
                    options.AreaViewLocationFormats.Add($"{Constants.DefaultControllersViewsPath}/Shared/{{0}}.cshtml");
                }
            });

            return services;
        }
    }
}