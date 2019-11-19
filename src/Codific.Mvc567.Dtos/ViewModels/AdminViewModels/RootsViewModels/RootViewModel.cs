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
using System.Linq;
using System.Threading.Tasks;

namespace Codific.Mvc567.Entities.ViewModels.AdminViewModels.RootsViewModels
{
    public class RootViewModel
    {
        public RootViewModel()
        {
            this.Folders = new List<RootFolderViewModel>();
            this.Files = new List<RootFileViewModel>();
        }

        public string Root { get; set; }

        public string FolderName { get; set; }

        public List<RootFolderViewModel> Folders { get; private set; }

        public List<RootFileViewModel> Files { get; private set; }

        public void AddFile(string name, string relativePath, long fileSize, DateTime createdOn, DateTime lastModifiedOn)
        {
            this.Files.Add(new RootFileViewModel
            {
                Name = name,
                RelativePath = relativePath,
                Size = fileSize,
                CreatedOn = createdOn,
                LastModifiedOn = lastModifiedOn,
            });
        }

        public void AddFolder(string name, string relativePath, DateTime createdOn, DateTime lastModifiedOn)
        {
            this.Folders.Add(new RootFolderViewModel
            {
                Name = name,
                RelativePath = relativePath,
                CreatedOn = createdOn,
                LastModifiedOn = lastModifiedOn,
            });
        }
    }
}
