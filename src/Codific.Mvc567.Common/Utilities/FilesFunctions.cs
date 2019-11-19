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
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Utilities
{
    public static class FilesFunctions
    {
        private static readonly string[] SizeSuffixes =
        {
            "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB",
        };

        public static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            }

            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }

            if (value == 0)
            {
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
            }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format(
                "{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        public static string GetUniqueFileName()
        {
            return $"f{CryptoFunctions.MD5Hash(Guid.NewGuid().ToString()).ToLower()}";
        }

        public static FileExtensions GetFileExtension(string extension)
        {
            string enumStandardExtension = $"_{extension.Replace(".", string.Empty)}";
            FileExtensions result = (FileExtensions)Enum.Parse(typeof(FileExtensions), enumStandardExtension, true);
            return result;
        }

        public static FileTypes GetFileType(FileExtensions fileExtension)
        {
            int fileExtensionCode = (int)fileExtension;

            int fileTypeCode = ((int)((double)fileExtensionCode / 100)) - 9;

            return (FileTypes)fileTypeCode;
        }
    }
}