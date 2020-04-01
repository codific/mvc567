using System;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Attributes
{
    public class DetailsFieldAttribute : Attribute
    {
        public DetailsFieldAttribute(DetailsFiledType type)
        {
            this.Type = type;
        }

        public DetailsFiledType Type { get; set; }
    }
}