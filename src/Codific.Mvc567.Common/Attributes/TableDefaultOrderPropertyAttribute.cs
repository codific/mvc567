using System;
using System.Runtime.CompilerServices;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Attributes
{
    public class TableDefaultOrderPropertyAttribute : Attribute
    {
        public TableDefaultOrderPropertyAttribute(FilterOrderType orderType, [CallerMemberName] string propertyName = null)
        {
            this.OrderType = orderType;
            this.PropertyName = propertyName;
        }

        public FilterOrderType OrderType { get; }

        public string PropertyName { get; }
    }
}