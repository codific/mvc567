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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mvc567.DataAccess.Identity
{
    public static class ApplicationPermissions
    {
        public static ReadOnlyCollection<ApplicationPermission> AllPermissions;

        //Policies
        public const string AccessAdministrationPolicy = "Access Administration";
        public const string AccessErrorLogsPolicy = "Access Error Logs";

        public const string UsersManagementPolicy = "Users Management";

        public const string PublicRootAccessPolicy = "Public Root Access";
        public const string PrivateRootAccessPolicy = "Private Root Access";

        public const string StaticPageManagementPolicy = "Static Page Management";
        public const string SearchEngineOptimizationManagementPolicy = "Search Engine Optimization Management";
        public const string LanguagesManagementPolicy = "Languages Management";

        //Permissions
        public static ApplicationPermission AccessAdministration = new ApplicationPermission(AccessAdministrationPolicy);
        public static ApplicationPermission AccessErrorLogs = new ApplicationPermission(AccessErrorLogsPolicy);

        public static ApplicationPermission UsersManagement = new ApplicationPermission(UsersManagementPolicy);

        public static ApplicationPermission PublicRootAccess = new ApplicationPermission(PublicRootAccessPolicy);
        public static ApplicationPermission PrivateRootAccess = new ApplicationPermission(PrivateRootAccessPolicy);

        public static ApplicationPermission StaticPageManagement = new ApplicationPermission(StaticPageManagementPolicy);
        public static ApplicationPermission SearchEngineOptimizationManagement = new ApplicationPermission(SearchEngineOptimizationManagementPolicy);
        public static ApplicationPermission LanguagesManagement = new ApplicationPermission(LanguagesManagementPolicy);

        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
                AccessAdministration,
                AccessErrorLogs,

                UsersManagement,

                PublicRootAccess,
                PrivateRootAccess,

                StaticPageManagement,
                SearchEngineOptimizationManagement,
                LanguagesManagement
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.Where(p => p.Name == permissionName).FirstOrDefault();
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.Where(p => p.Value == permissionValue).FirstOrDefault();
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }
    }
}
