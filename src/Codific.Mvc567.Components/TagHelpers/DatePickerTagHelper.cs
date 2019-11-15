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

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("date-picker", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DatePickerTagHelper : TagHelper
    {
        [HtmlAttributeName("name")]
        public string ModelName { get; set; }

        [HtmlAttributeName("value")]
        public string Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
        }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
            return base.ProcessAsync(context, output);
        }

        public string RenderTag()
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            Guid itemId = Guid.NewGuid();

            contentStringBuilder.Append($"<div id=\"datepicker-popup-{itemId}\" class=\"input-group date datepicker datepicker-popup\">");
            contentStringBuilder.Append($"<input type=\"text\" class=\"form-control\" name=\"{ModelName}\" value=\"{Value}\">");
            contentStringBuilder.Append("<span class=\"input-group-addon input-group-append border-left\">");
            contentStringBuilder.Append("<span class=\"mdi mdi-calendar input-group-text\"></span>");
            contentStringBuilder.Append("</span>");
            contentStringBuilder.Append("</div>");

            return contentStringBuilder.ToString();
        }
    }
}
