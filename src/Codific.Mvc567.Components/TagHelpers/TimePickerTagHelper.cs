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

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("time-picker", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TimePickerTagHelper : TagHelper
    {
        [HtmlAttributeName("name")]
        public string ModelName { get; set; }

        [HtmlAttributeName("value")]
        public string Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            string tagContent = this.RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            string tagContent = this.RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
            return base.ProcessAsync(context, output);
        }

        public string RenderTag()
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            Guid itemId = Guid.NewGuid();

            contentStringBuilder.Append($"<div class=\"input-group date timepicker-popup\" id=\"timepicker-{itemId}\" data-target-input=\"nearest\">");
            contentStringBuilder.Append($"<div class=\"input-group\" data-target=\"#timepicker-{itemId}\" data-toggle=\"datetimepicker\">");
            contentStringBuilder.Append($"<input type=\"text\" name=\"{this.ModelName}\" class=\"form-control datetimepicker-input\" value=\"{this.Value}\" data-target=\"#timepicker-{itemId}\" />");
            contentStringBuilder.Append("<div class=\"input-group-addon input-group-append\">");
            contentStringBuilder.Append("<i class=\"mdi mdi-clock input-group-text\"></i>");
            contentStringBuilder.Append("</div>");
            contentStringBuilder.Append("</div>");
            contentStringBuilder.Append("</div>");

            return contentStringBuilder.ToString();
        }
    }
}