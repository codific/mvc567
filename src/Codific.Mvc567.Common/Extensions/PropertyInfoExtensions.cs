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
using System.Reflection;
using System.Text;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<T>(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(T), true).Any();
        }

        public static bool IsNullableType(this Type objectType)
        {
            return Nullable.GetUnderlyingType(objectType) != null;
        }

        public static bool IsNumericType(this Type objectType)
        {
            switch (Type.GetTypeCode(objectType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static T GetAttribute<T>(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.HasAttribute<T>())
            {
                return (T)propertyInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault();
            }

            return default(T);
        }

        public static SaveDirectorySettings? GetSaveDirectorySettingsFromAttribute(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.HasAttribute<SaveDirectoryAttribute>())
            {
                var attribute = propertyInfo.GetAttribute<SaveDirectoryAttribute>();

                SaveDirectorySettings settings = new SaveDirectorySettings
                {
                    RelativePath = Path.Combine(attribute.DirectoryFolders),
                    Root = attribute.Root,
                    UserSpecific = attribute.UserSpecific,
                };

                return settings;
            }

            return null;
        }

        public struct SaveDirectorySettings
        {
            public string RelativePath { get; set; }

            public ApplicationRoots Root { get; set; }

            public bool UserSpecific { get; set; }
        }
    }
}
