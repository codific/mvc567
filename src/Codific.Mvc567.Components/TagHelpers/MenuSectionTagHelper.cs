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

using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("menu-section", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MenuSectionTagHelper : TagHelper
    {
        [HtmlAttributeName("controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("icon")]
        public string Icon { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("href")]
        public string Href { get; set; }

        [HtmlAttributeName("single")]
        public bool Single { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            this.ProcessOutput(ref output);

            base.Process(context, output);
        }

        private void ProcessOutput(ref TagHelperOutput output)
        {
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            bool collapsed = this.ViewContext.RouteData.Values["controller"].ToString().ToLower() != this.Controller.ToLower();
            string activeClass = (!collapsed) ? "active" : string.Empty;
            output.Attributes.Add(new TagHelperAttribute("class", $"nav-item main-nav-item {activeClass}", HtmlAttributeValueStyle.DoubleQuotes));

            StringBuilder preContent = new StringBuilder();
            if (this.Single)
            {
                preContent.Append($"<a class=\"nav-link\" title=\"{this.Title}\" href=\"{this.Href}\">");
            }
            else
            {
                string collapsedClass = collapsed ? "collapsed" : string.Empty;
                preContent.Append($"<a class=\"nav-link {collapsedClass}\" title=\"{this.Title}\" data-toggle=\"collapse\" href=\"#{this.Controller.ToLower()}\" aria-controls=\"{this.Controller.ToLower()}\" aria-expanded=\"{(!collapsed).ToString().ToLower()}\">");
            }

            preContent.Append($"<i class=\"menu-icon {this.Icon}\"></i>");
            preContent.Append($"<span class=\"menu-title\">{this.Title}</span>");
            if (!this.Single)
            {
                preContent.Append($"<i class=\"menu-arrow\"></i>");
            }

            preContent.Append($"</a>");
            if (!this.Single)
            {
                string showedClass = !collapsed ? "show" : string.Empty;
                preContent.Append($"<div class=\"collapse {showedClass}\" id=\"{this.Controller.ToLower()}\">");
                preContent.Append($"<ul class=\"nav flex-column sub-menu\">");

                output.PostContent.AppendHtml(new HtmlString($"</ul>"));
                output.PostContent.AppendHtml(new HtmlString($"</div>"));
            }

            output.PreContent.AppendHtml(new HtmlString(preContent.ToString()));
        }
    }
}