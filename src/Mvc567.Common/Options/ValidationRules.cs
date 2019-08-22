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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mvc567.Common.Options
{
    public class ValidationRules
    {
        private List<int> allowedFormatExtensions = new List<int>();
        private List<FileExtensions> allowedFormatExtensionsAsEnum = new List<FileExtensions>();

        [JsonProperty("MaxAllowedFileSize")]
        public long MaxAllowedFileSize { get; set; }

        [JsonProperty("AllowedFormatExtensions")]
        public List<int> AllowedFormatExtensions
        {
            get
            {
                return this.allowedFormatExtensions;
            }
            set
            {
                this.allowedFormatExtensions = value;
            }
        }

        [JsonProperty("AllowedMimeTypes")]
        public List<string> AllowedMimeTypes { get; set; }

        public List<FileExtensions> AllowedFormatExtensionsAsEnum
        {
            get
            {
                ParseFileExtensions();
                return this.allowedFormatExtensionsAsEnum;
            }
        }

        private void ParseFileExtensions()
        {
            if (this.allowedFormatExtensions != null)
            {
                this.allowedFormatExtensionsAsEnum = new List<FileExtensions>();
                foreach (var extensionCode in this.allowedFormatExtensions)
                {
                    FileExtensions tempExtension;
                    if (Enum.TryParse<FileExtensions>(extensionCode.ToString(), out tempExtension))
                    {
                        this.allowedFormatExtensionsAsEnum.Add(tempExtension);
                    }
                }
            }
        }
    }
}
