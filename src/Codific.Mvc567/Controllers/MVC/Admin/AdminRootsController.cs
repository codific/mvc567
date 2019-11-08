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

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Entities.DataTransferObjects.ServiceResults;
using Codific.Mvc567.Entities.ViewModels.AdminViewModels.RootsViewModels;
using Codific.Mvc567.Entities.ViewModels.Mapping;
using Codific.Mvc567.Services.Infrastructure;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/roots/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ValidateAdminCookie]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminRootsController : Controller
    {
        private readonly IFileSystemService fileSystemService;

        public AdminRootsController(IFileSystemService fileSystemService)
        {
            this.fileSystemService = fileSystemService;
        }

        [HttpGet]
        [Route("public/{*folders}")]
        [Authorize(Policy = ApplicationPermissions.PublicRootAccessPolicy)]
        public async Task<IActionResult> PublicRoot(string[] folders = null)
        {
            IEnumerable<FileSystemItem> fileSystemItems = null;
            RootViewModel model = null;
            if (folders == null)
            {
                fileSystemItems = await this.fileSystemService.ScanPublicDirectoryAsync();
                model = RootMapper.Map("Public Root", fileSystemItems);
            }
            else
            {
                string folderPath = string.Join("\\", folders);
                fileSystemItems = await this.fileSystemService.ScanDirectoryAsync(Path.Combine(this.fileSystemService.PublicRootDirectory, folderPath), this.fileSystemService.PublicRootDirectory);
                model = RootMapper.Map(folderPath, fileSystemItems);
            }
            
            model.Root = "Public";

            return View("Root", model);
        }

        [HttpGet]
        [Route("private/{*folders}")]
        [Authorize(Policy = ApplicationPermissions.PrivateRootAccessPolicy)]
        public async Task<IActionResult> PrivateRoot(string[] folders = null)
        {
            IEnumerable<FileSystemItem> fileSystemItems = null;
            RootViewModel model = null;
            if (folders == null)
            {
                fileSystemItems = await this.fileSystemService.ScanPrivateDirectoryAsync();
                model = RootMapper.Map("Private Root", fileSystemItems);
            }
            else
            {
                string folderPath = string.Join("\\", folders);
                fileSystemItems = await this.fileSystemService.ScanDirectoryAsync(Path.Combine(this.fileSystemService.PrivateRootDirectory, folderPath), this.fileSystemService.PrivateRootDirectory);
                model = RootMapper.Map(folderPath, fileSystemItems);
            }

            model.Root = "Private";
            return View("Root", model);
        }

        [HttpGet]
        [Route("public/file/{*folders}")]
        [Authorize(Policy = ApplicationPermissions.PublicRootAccessPolicy)]
        public async Task<IActionResult> PublicRootFile(string[] folders = null)
        {
            IActionResult result = NotFound();
            if (folders != null)
            {
                string filePath = string.Join("\\", folders);
                var fileResult = await this.fileSystemService.GetFileAsync(Path.Combine(this.fileSystemService.PublicRootDirectory, filePath), this.fileSystemService.PublicRootDirectory);

                result = File(fileResult.Stream, fileResult.ContentType);
            }

            return result;
        }

        [HttpGet]
        [Route("private/file/{*folders}")]
        [Authorize(Policy = ApplicationPermissions.PrivateRootAccessPolicy)]
        public async Task<IActionResult> PrivateRootFile(string[] folders = null)
        {
            IActionResult result = NotFound();
            if (folders != null)
            {
                string filePath = string.Join("\\", folders);
                var fileResult = await this.fileSystemService.GetFileAsync(Path.Combine(this.fileSystemService.PrivateRootDirectory, filePath), this.fileSystemService.PrivateRootDirectory);

                result = File(fileResult.Stream, fileResult.ContentType);
            }

            return result;
        }
    }
}
