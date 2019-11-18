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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Utilities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("enum-checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class EnumCheckboxTagHelper : TagHelper
    {
        [HtmlAttributeName("enum")]
        public Type Enum { get; set; }

        [HtmlAttributeName("selected-values")]
        public int[] SelectedValues { get; set; }

        [HtmlAttributeName("model-name")]
        public string ModelName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "row m-0");
            string tagContent = this.RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "row m-0");
            string tagContent = this.RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
            return base.ProcessAsync(context, output);
        }

        private string RenderTag()
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            var enumDictionary = EnumFunctions.GetEnumList(this.Enum);
            foreach (var enumItem in enumDictionary)
            {
                contentStringBuilder.Append(this.GetRadioItemHtml(enumItem.Value, enumItem.Key, this.SelectedValues?.Contains(enumItem.Key)));
            }

            return contentStringBuilder.ToString();
        }

        private string GetRadioItemHtml(string name, int value, bool? selected)
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            contentStringBuilder.Append("<div class=\"form-check mr-3 mt-1 mb-2 form-check-flat\">");
            contentStringBuilder.Append("<label class=\"form-check-label\">");
            contentStringBuilder.Append($"<input type=\"checkbox\" class=\"form-check-input\" name=\"{this.ModelName}[]\" id=\"flatCheckbox-{Guid.NewGuid().ToString()}\" {(selected.HasValue && selected.Value ? "checked=\"checked\"" : string.Empty)} value=\"{value}\"> {name} <i class=\"input-helper\"></i>");
            contentStringBuilder.Append("</label>");
            contentStringBuilder.Append("</div>");

            return contentStringBuilder.ToString();
        }
    }
}