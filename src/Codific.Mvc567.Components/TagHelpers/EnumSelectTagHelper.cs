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
using System.Text;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Utilities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("enum-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class EnumSelectTagHelper : TagHelper
    {
        [HtmlAttributeName("enum")]
        public Type Enum { get; set; }

        [HtmlAttributeName("selected-value")]
        public int SelectedValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string selectTag = this.RenderSelectTag();
            output.Content.SetHtmlContent(new HtmlString(selectTag));
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string selectTag = this.RenderSelectTag();
            output.Content.SetHtmlContent(new HtmlString(selectTag));
            return base.ProcessAsync(context, output);
        }

        private string RenderSelectTag()
        {
            StringBuilder optionsStringBuilder = new StringBuilder();
            var enumDictionary = EnumFunctions.GetEnumList(this.Enum);
            foreach (var enumItem in enumDictionary)
            {
                string selectedAttribute = string.Empty;
                if (enumItem.Key == this.SelectedValue)
                {
                    selectedAttribute = "selected ";
                }

                optionsStringBuilder.Append($"<option value=\"{enumItem.Key}\" {selectedAttribute}>{enumItem.Value}</option>");
            }

            return optionsStringBuilder.ToString();
        }
    }
}