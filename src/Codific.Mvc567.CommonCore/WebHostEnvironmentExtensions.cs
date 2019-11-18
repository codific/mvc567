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
using System.IO;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace Codific.Mvc567.CommonCore
{
    public static class WebHostEnvironmentExtensions
    {
        public static string GetPrivateRootUserUploadDirectory(
            this IWebHostEnvironment hostingEnvironment,
            Guid? userId = null)
        {
            string userFolder = CryptoFunctions.MD5Hash(userId.ToString()).ToLower();
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.UsersFolderName, userFolder);
        }

        public static string GetPrivateRootGlobalUploadDirectory(this IWebHostEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.GlobalFolderName);
        }

        public static string GetPrivateRootTempUploadDirectory(this IWebHostEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.GetPrivateRoot(), Constants.UploadFolderName, Constants.TempFolderName);
        }

        public static string GetPrivateRoot(this IWebHostEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.ContentRootPath, Constants.PrivateRootFolderName);
        }

        public static string GetPublicRootGlobalContentDirectory(this IWebHostEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.ContentFolderName, Constants.GlobalFolderName);
        }

        public static string GetPublicRootUserContentDirectory(this IWebHostEnvironment hostingEnvironment, Guid userId)
        {
            string userFolder = CryptoFunctions.MD5Hash(userId.ToString()).ToLower();
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.ContentFolderName, Constants.UsersFolderName, userFolder);
        }

        public static string GetLanguagesDirectory(this IWebHostEnvironment hostingEnvironment)
        {
            return Path.Combine(hostingEnvironment.WebRootPath, Constants.LanguagesFolderName);
        }
    }
}