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

using Codific.Mvc567.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions
{
    public class CreateEditInputViewModel
    {
        public string Label { get; set; }

        public string Name { get; set; }

        public CreateEntityInputType Type { get; set; }

        public Type EnumType { get; set; }

        public object Value { get; set; }

        public Guid GuidValue
        {
            get
            {
                Guid resultGuid = Guid.Empty;
                if (Value != null)
                {
                    Guid.TryParse(Value.ToString(), out resultGuid);
                }

                return resultGuid;
            }
        }

        public Guid[] GuidArrayValue
        {
            get
            {
                List<Guid> guidArray = new List<Guid>();
                if (Value != null)
                {
                    object[] tempArray = (object[])Value;
                    foreach (var arrayItem in tempArray)
                    {
                        Guid tempGuid = Guid.Empty;
                        if (Guid.TryParse(arrayItem.ToString(), out tempGuid))
                        {
                            guidArray.Add(tempGuid);
                        }
                    }
                }

                return guidArray.ToArray();
            }
        }

        public Type DatabaseEntityType { get; set; }

        public string DatabaseEntityVisibleProperty { get; set; }

        public bool IsNullable
        {
            get
            {
                if (Value == null)
                {
                    return true;
                }
                else
                {
                    return Nullable.GetUnderlyingType(Value.GetType()) != null;
                }
            }
        }
    }
}
