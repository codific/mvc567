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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Entities.DataTransferObjects.Entities;
using Codific.Mvc567.Entities.DataTransferObjects.ServiceResults;
using Codific.Mvc567.Services.Abstractions;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class FileSystemService : AbstractService, IFileSystemService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        
        public FileSystemService(IUnitOfWork uow, IMapper mapper, IHostingEnvironment hostingEnvironment) : base(uow, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string PublicRootDirectory => this.hostingEnvironment.WebRootPath;

        public string PrivateRootDirectory => Path.Combine(this.hostingEnvironment.ContentRootPath, Constants.PrivateRootFolderName);

        public async Task<FileSystemResult> GetFileAsync(string filePath, string baseDirectory = "")
        {
            if (!(filePath.StartsWith(PublicRootDirectory) || filePath.StartsWith(PrivateRootDirectory)))
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

        public async Task<FileDto> GetFileByIdAsync(Guid id)
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var file = await standardRepository.GetAsync<Mvc567.Entities.Database.File>(id);
                var fileDto = this.mapper.Map<FileDto>(file);

                return fileDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FileDto GetFileById(Guid id)
        {
            try
            {
                var standardRepository = this.uow.GetStandardRepository();
                var file = standardRepository.Get<Mvc567.Entities.Database.File>(id);
                var fileDto = this.mapper.Map<FileDto>(file);

                return fileDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<FileSystemItem>> ScanDirectoryAsync(string directory, string baseDirectory = "")
        {
            if (!(directory.StartsWith(PublicRootDirectory) || directory.StartsWith(PrivateRootDirectory)))
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
            return await ScanDirectoryAsync(PrivateRootDirectory, PrivateRootDirectory);
        }

        public async Task<IEnumerable<FileSystemItem>> ScanPublicDirectoryAsync()
        {
            return await ScanDirectoryAsync(PublicRootDirectory, PublicRootDirectory);
        }

        public async Task<FileDto> UploadFileAsync(IFormFile formFile)
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
                    FileExtension = FilesFunctions.GetFileExtension(resultFileExtension)
                };
                fileEntity.FileType = FilesFunctions.GetFileType(fileEntity.FileExtension);


                this.uow.GetStandardRepository().Add<Mvc567.Entities.Database.File>(fileEntity);
                await this.uow.SaveChangesAsync();

                var fileDto = this.mapper.Map<FileDto>(fileEntity);
                return fileDto;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(UploadFileAsync));
                return null;
            }
        }
    }
}
