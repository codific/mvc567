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

using Microsoft.AspNetCore.Hosting;
using Codific.Mvc567.Common.Utilities;
using System;
using System.IO;

namespace Codific.Mvc567.Common.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static string GetPrivateRootUserUploadDirectory(this IHostingEnvironment hostingEnvironment, Guid? userId = null)
        {
            string userFolder = CryptoFunctions.MD5Hash(userId.ToString()).ToLower();
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.UsersFolderName, userFolder);
        }

        public static string GetPrivateRootGlobalUploadDirectory(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.GlobalFolderName);
        }

        public static string GetPrivateRootTempUploadDirectory(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.TempFolderName);
        }

        public static string GetPrivateRoot(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.ContentRootPath, Constants.PrivateRootFolderName);
        }

        public static string GetPublicRootGlobalContentDirectory(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.ContentFolderName, Constants.GlobalFolderName);
        }

        public static string GetPublicRootUserContentDirectory(this IHostingEnvironment hostingEnvironment, Guid userId)
        {
            string userFolder = CryptoFunctions.MD5Hash(userId.ToString()).ToLower();
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.ContentFolderName, Constants.UsersFolderName, userFolder);
        }

        public static string GetLanguagesDirectory(this IHostingEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.LanguagesFolderName);
        }
    }
}
