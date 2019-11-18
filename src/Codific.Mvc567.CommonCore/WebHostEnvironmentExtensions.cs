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