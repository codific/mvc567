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

using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("sidebar-navigation-link", TagStructure = TagStructure.WithoutEndTag)]
    public class SidebarNavigationLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public SidebarNavigationLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("area")]
        public string Area { get; set; }

        [HtmlAttributeName("action")]
        public string Action { get; set; }

        [HtmlAttributeName("controller")]
        public string Controller { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);

            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add(new TagHelperAttribute("class", "nav-item", HtmlAttributeValueStyle.DoubleQuotes));
            var routeKeys = urlHelper.ActionContext.RouteData.Values.Select(x => x.Key);
            foreach (var routeKey in routeKeys)
            {
                if (routeKey != "area" && routeKey != "controller" && routeKey != "action")
                {
                    urlHelper.ActionContext.RouteData.Values.Remove(routeKey);
                }
            }

            string href = urlHelper.Action(this.Action, this.Controller, new { Area = this.Area });
            string activeClass = string.Empty;
            if (this.ViewContext.RouteData.Values["controller"].ToString().ToLower() == this.Controller.ToLower() &&
                this.ViewContext.RouteData.Values["action"].ToString().ToLower() == this.Action.ToLower() &&
                this.ViewContext.RouteData.Values["area"].ToString().ToLower() == this.Area.ToLower())
            {
                activeClass = "active";
            }

            output.Content.SetHtmlContent(new HtmlString($"<a class=\"nav-link {activeClass}\" title=\"{this.Title}\" href=\"{href}\">{this.Title}</a>"));
        }
    }
}