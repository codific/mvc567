using System;
using System.Runtime.CompilerServices;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Common.Attributes
{
    public class TableDefaultOrderPropertyAttribute : Attribute
    {
        public TableDefaultOrderPropertyAttribute(FilterOrderType orderType)
        {
            this.OrderType = orderType;
        }

        public FilterOrderType OrderType { get; }
    }
}