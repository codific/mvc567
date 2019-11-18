using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Codific.Mvc567.Common.Attributes
{
    public class BreadcrumbItem
    {
        private readonly IUrlHelper urlHelper;

        public BreadcrumbItem(ActionExecutingContext context)
        {
            this.urlHelper = new UrlHelper(context);
        }

        public string Title { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string ParameterName { get; set; }

        public string ParameterValue { get; set; }

        public string ActionLink
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Controller) && !string.IsNullOrEmpty(this.Action))
                {
                    if (!string.IsNullOrEmpty(this.ParameterName) && !string.IsNullOrEmpty(this.ParameterValue))
                    {
                        RouteValueDictionary parametersObject = new RouteValueDictionary
                        {
                            { this.ParameterName, this.ParameterValue },
                        };
                        return this.urlHelper.Action(this.Action, this.Controller, parametersObject);
                    }
                    else
                    {
                        return this.urlHelper.Action(this.Action, this.Controller);
                    }
                }

                return string.Empty;
            }
        }

        public int Order { get; set; }

        public bool Active { get; set; }
    }
}