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

namespace Mvc567.Common
{
    public static class Constants
    {
        public const string PrivateRootFolderName = "privateroot";
        public const string UploadFolderName = "uploads";
        public const string GlobalFolderName = "global";
        public const string UsersFolderName = "users";
        public const string ContentFolderName = "content";
        public const string TempFolderName = "temp";
        public const string AssetsFolderName = "assets";
        public const string ImagesFolderName = "images";
        public const string LanguagesFolderName = "locales";

        public const string DateFormat = "dddd, dd MMMM yyyy";
        public const string TimeFormat = "HH:mm";
        public const string DateTimeFormat = "dddd, dd MMMM yyyy HH:mm";

        public const string DefaultAreasViewsPath = "/Views/AreasViews";
        public const string DefaultControllersViewsPath = "/Views/ControllersViews";
        public const string DefaultEmailViewsPath = "/Views/EmailViews";

        public const string ControllerStaticPageRoute = "{*route:regex(^([[a-zA-Z0-9-/]]+)$)}";

        public const string LanguageControllerRouteKey = "language";
        public const string LanguageControllerPageRoute = "{" + LanguageControllerRouteKey + ":length(2,2)}";
        public const string LanguageControllerStaticPageRoute = LanguageControllerPageRoute + "/" + ControllerStaticPageRoute;

        public const string LanguageCookieName = ".Mvc567.Language";

    }
}
