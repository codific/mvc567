// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Georgi Karagogov
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

namespace Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("bool-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BoolSelectTagHelper : TagHelper
    {
        [HtmlAttributeName("value")]
        public bool Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
        }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
            return base.ProcessAsync(context, output);
        }

        public string RenderTag()
        {
            StringBuilder contentStringBuilder = new StringBuilder();

            if (Value)
            {
                contentStringBuilder.Append("<option selected=\"selected\" value=\"True\">Yes</option>");
                contentStringBuilder.Append("<option value=\"False\">No</option>");
            }
            else
            {
                contentStringBuilder.Append("<option value=\"True\">Yes</option>");
                contentStringBuilder.Append("<option selected=\"selected\" value=\"False\">No</option>");
            }

            return contentStringBuilder.ToString();
        }
    }
}
