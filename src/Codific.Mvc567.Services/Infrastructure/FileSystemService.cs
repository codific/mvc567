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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.CommonCore;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class FileSystemService : Service, IFileSystemService
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public FileSystemService(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment hostingEnvironment)
            : base(uow, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string PublicRootDirectory => this.hostingEnvironment.WebRootPath;

        public string PrivateRootDirectory => Path.Combine(this.hostingEnvironment.ContentRootPath, Constants.PrivateRootFolderName);

        public async Task<FileSystemResult> GetFileAsync(string filePath, string baseDirectory = "")
        {
            var doesFileStartWithRootDirectory = await Task.Factory.StartNew<bool>(() => filePath.StartsWith(this.PublicRootDirectory) || filePath.StartsWith(this.PrivateRootDirectory));
            if (!doesFileStartWithRootDirectory)
            {
                return null;
            }

            if (File.Exists(filePath))
            {
                FileSystemResult fileSystemResult = new FileSystemResult();
                var fileInfo = new FileInfo(filePath);
                string contentType = string.Empty;
                new FileExtensionContentTypeProvider().TryGetContentType(fileInfo.Name, out contentType);
                fileSystemResult.ContentType = contentType;
                fileSystemResult.Stream = File.OpenRead(filePath);

                return fileSystemResult;
            }

            return null;
        }

        public async Task<TFileModel> GetFileByIdAsync<TFileModel>(Guid id)
        {
            try
            {
                var file = await this.StandardRepository.GetAsync<Mvc567.Entities.Database.File>(id);
                var fileDto = this.Mapper.Map<TFileModel>(file);

                return fileDto;
            }
            catch (Exception)
            {
                return default(TFileModel);
            }
        }

        public TFileModel GetFileById<TFileModel>(Guid id)
        {
            try
            {
                var file = this.StandardRepository.Get<Mvc567.Entities.Database.File>(id);
                var fileDto = this.Mapper.Map<TFileModel>(file);

                return fileDto;
            }
            catch (Exception)
            {
                return default(TFileModel);
            }
        }

        public async Task<IEnumerable<FileSystemItem>> ScanDirectoryAsync(string directory, string baseDirectory = "")
        {
            var doesDirectoryStartWithRootDirectory = await Task.Factory.StartNew<bool>(() => directory.StartsWith(this.PublicRootDirectory) || directory.StartsWith(this.PrivateRootDirectory));
            if (!doesDirectoryStartWithRootDirectory)
            {
                return null;
            }

            List<FileSystemItem> fileSystemItems = new List<FileSystemItem>();
            var files = Directory.GetFiles(directory);
            var folders = Directory.GetDirectories(directory);

            foreach (var folder in folders)
            {
                FileSystemItem currentFileSystemItem = new FileSystemItem();
                if (Directory.Exists(folder))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(folder);
                    currentFileSystemItem.Type = FileSystemItemType.Folder;
                    currentFileSystemItem.Name = directoryInfo.Name;
                    currentFileSystemItem.Path = folder;
                    currentFileSystemItem.RelativePath = folder.Replace(baseDirectory, string.Empty);
                    currentFileSystemItem.CreatedOn = directoryInfo.CreationTime;
                    currentFileSystemItem.LastModifiedOn = directoryInfo.LastWriteTime;
                }

                fileSystemItems.Add(currentFileSystemItem);
            }

            foreach (var file in files)
            {
                FileSystemItem currentFileSystemItem = new FileSystemItem();
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    currentFileSystemItem.Type = FileSystemItemType.File;
                    currentFileSystemItem.Name = fileInfo.Name;
                    currentFileSystemItem.Path = file;
                    currentFileSystemItem.RelativePath = file.Replace(baseDirectory, string.Empty);
                    currentFileSystemItem.FileSize = fileInfo.Length;
                    currentFileSystemItem.CreatedOn = fileInfo.CreationTime;
                    currentFileSystemItem.LastModifiedOn = fileInfo.LastWriteTime;
                }

                fileSystemItems.Add(currentFileSystemItem);
            }

            return fileSystemItems;
        }

        public async Task<IEnumerable<FileSystemItem>> ScanPrivateDirectoryAsync()
        {
            return await this.ScanDirectoryAsync(this.PrivateRootDirectory, this.PrivateRootDirectory);
        }

        public async Task<IEnumerable<FileSystemItem>> ScanPublicDirectoryAsync()
        {
            return await this.ScanDirectoryAsync(this.PublicRootDirectory, this.PublicRootDirectory);
        }

        public async Task<TFileModel> UploadFileAsync<TFileModel>(IFormFile formFile)
        {
            try
            {
                string saveDirectory = this.hostingEnvironment.GetPrivateRootTempUploadDirectory();

                string resultFileName = FilesFunctions.GetUniqueFileName();
                string resultFileExtension = formFile.FileName.Split('.').LastOrDefault();
                string relativeSaveDirectory = Path.Combine(Constants.PrivateRootFolderName, Constants.UploadFolderName, Constants.TempFolderName);
                string fileFullPath = Path.Combine(saveDirectory, $"{resultFileName}.{resultFileExtension}");
                string fileRelativePath = Path.Combine(relativeSaveDirectory, $"{resultFileName}.{resultFileExtension}");

                using (FileStream stream = System.IO.File.Create(fileFullPath))
                {
                    formFile.CopyTo(stream);
                    stream.Flush();
                }

                Mvc567.Entities.Database.File fileEntity = new Mvc567.Entities.Database.File
                {
                    Name = resultFileName,
                    Path = fileRelativePath,
                    Temp = true,
                    FileExtension = FilesFunctions.GetFileExtension(resultFileExtension),
                };
                fileEntity.FileType = FilesFunctions.GetFileType(fileEntity.FileExtension);

                this.StandardRepository.Add<Mvc567.Entities.Database.File>(fileEntity);
                await this.Uow.SaveChangesAsync();

                var fileDto = this.Mapper.Map<TFileModel>(fileEntity);
                return fileDto;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.UploadFileAsync));
                return default(TFileModel);
            }
        }
    }
}