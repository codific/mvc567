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
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("file-uploader", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FileUploaderTagHelper : TagHelper
    {
        [HtmlAttributeName("name")]
        public string ModelName { get; set; }

        [HtmlAttributeName("value")]
        public Guid? Value { get; set; }

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
            string valueAttribute = string.Empty;
            if (this.Value.HasValue && this.Value.Value != Guid.Empty)
            {
                valueAttribute = $" value=\"{this.Value.Value}\"";
            }

            contentStringBuilder.Append($"<input type=\"file\" id=\"{itemId}\" class=\"upload-file-input\"/>");
            contentStringBuilder.Append($"<input type=\"hidden\"{valueAttribute} id=\"target-{itemId}\" name=\"{this.ModelName}\"/>");

            return contentStringBuilder.ToString();
        }
    }
}