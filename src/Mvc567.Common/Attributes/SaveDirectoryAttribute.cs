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

using Mvc567.Common.Enums;
using System;

namespace Mvc567.Common.Attributes
{
    public class SaveDirectoryAttribute : Attribute
    {
        public SaveDirectoryAttribute(ApplicationRoots root, params string[] directoryFolders)
        {
            Root = root;
            DirectoryFolders = directoryFolders;
        }

        public string[] DirectoryFolders { get; set; }

        public ApplicationRoots Root { get; set; }

        public bool UserSpecific { get; set; }
    }
}
