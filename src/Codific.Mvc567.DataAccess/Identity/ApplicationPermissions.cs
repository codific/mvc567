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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Codific.Mvc567.DataAccess.Identity
{
    public static class ApplicationPermissions
    {
        // Policies
        public const string AccessAdministrationPolicy = "Access Administration";
        public const string AccessErrorLogsPolicy = "Access Error Logs";
        public const string UsersManagementPolicy = "Users Management";
        public const string LanguagesManagementPolicy = "Languages Management";
        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions;

        // Permissions
        public static readonly ApplicationPermission AccessAdministration = new ApplicationPermission(AccessAdministrationPolicy);
        private static readonly ApplicationPermission AccessErrorLogs = new ApplicationPermission(AccessErrorLogsPolicy);
        private static readonly ApplicationPermission UsersManagement = new ApplicationPermission(UsersManagementPolicy);
        private static readonly ApplicationPermission LanguagesManagement = new ApplicationPermission(LanguagesManagementPolicy);

        static ApplicationPermissions()
        {
            List<ApplicationPermission> allPermissions = new List<ApplicationPermission>()
            {
                AccessAdministration,
                AccessErrorLogs,
                UsersManagement,
                LanguagesManagement,
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.FirstOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.FirstOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }
    }
}
