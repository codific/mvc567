using System;
using System.Runtime.CompilerServices;

namespace Codific.Mvc567.Common.Attributes
{
    public class SortablePropertyAttribute : Attribute
    {
        public SortablePropertyAttribute([CallerMemberName] string propertyName = null)
        {
            this.OrderArgument = propertyName;
        }

        public string OrderArgument { get; set; }
    }
}