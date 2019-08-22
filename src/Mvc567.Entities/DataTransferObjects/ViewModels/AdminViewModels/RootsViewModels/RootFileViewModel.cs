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

using System;
using Mvc567.Common.Utilities;

namespace Mvc567.Entities.ViewModels.AdminViewModels.RootsViewModels
{
    public class RootFileViewModel
    {
        public string Name { get; set; }

        public string RelativePath { get; set; }

        public string RelativePathUrlFormat
        {
            get
            {
                return RelativePath.Substring(1).Replace("\\", "/");
            }
        }

        public long Size { get; set; }

        public string SizeString
        {
            get
            {
                return FilesFunctions.SizeSuffix(Size, 2);
            }
        }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }
    }
}
