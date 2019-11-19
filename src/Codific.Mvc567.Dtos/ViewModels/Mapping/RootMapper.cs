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
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Entities.ViewModels.AdminViewModels.RootsViewModels;

namespace Codific.Mvc567.Entities.ViewModels.Mapping
{
    public static class RootMapper
    {
        public static RootViewModel Map(string folderName, IEnumerable<FileSystemItem> fileSystemItems)
        {
            RootViewModel model = new RootViewModel
            {
                FolderName = folderName,
            };

            foreach (var item in fileSystemItems)
            {
                if (item.Type == FileSystemItemType.File)
                {
                    model.AddFile(item.Name, item.RelativePath, item.FileSize, item.CreatedOn, item.LastModifiedOn);
                }
                else if (item.Type == FileSystemItemType.Folder)
                {
                    model.AddFolder(item.Name, item.RelativePath, item.CreatedOn, item.LastModifiedOn);
                }
            }

            return model;
        }
    }
}
