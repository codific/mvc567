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
using System.Text;
using Codific.Mvc567.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Codific.Mvc567.Common.Attributes
{
    public class BreadcrumbAttribute : ActionFilterAttribute
    {
        private string title;
        private string controller;
        private string action;
        private string parameterName;
        private string parameterValue;
        private int order;
        private bool active;

        public BreadcrumbAttribute(string title, bool active, int order, string action = null, string controller = null, string parameterName = null, string parameterValue = null)
        {
            this.title = title;
            this.active = active;
            this.action = action;
            this.controller = controller;
            this.order = order;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (((Controller)context.Controller).ViewData["breadcrumbs"] == null)
            {
                ((Controller)context.Controller).ViewData["breadcrumbs"] = new List<BreadcrumbItem>();
            }

            if (this.active && !string.IsNullOrEmpty(this.action) && string.IsNullOrEmpty(this.controller))
            {
                this.controller = ((Controller)context.Controller).GetType().Name.Replace("Controller", string.Empty);
            }

            var currentBreadcrumb = new BreadcrumbItem(context)
            {
                Title = this.title,
                Controller = this.controller,
                Action = this.action,
                ParameterName = this.parameterName,
                ParameterValue = this.parameterValue,
                Order = this.order,
                Active = this.active,
            };

            ((List<BreadcrumbItem>)((Controller)context.Controller).ViewData["breadcrumbs"]).Add(currentBreadcrumb);
            base.OnActionExecuting(context);
        }
    }
}
