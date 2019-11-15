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
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("form-checkbox-input", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormCheckboxInputTagHelper : AbstractFormComponentTagHelper
    {
        public FormCheckboxInputTagHelper(IHtmlGenerator htmlGenerator) : base(htmlGenerator)
        {

        }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("labels-class")]
        public string LabelsClass { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add(new TagHelperAttribute("class", $"{Class} form-check form-check-flat".Trim()));

            StringBuilder outputStringBuilder = new StringBuilder();

            outputStringBuilder.Append($"<label class=\"{($"{LabelsClass} form-check-label").Trim()}\">");
            outputStringBuilder.Append(await RenderCheckboxInputAsync());
            outputStringBuilder.Append($"{Label} <i class=\"input-helper\"></i>");
            outputStringBuilder.Append("</label>");
            outputStringBuilder.Append(await RenderValidationTagHelperAsync());
            output.Content.SetHtmlContent(new HtmlString(outputStringBuilder.ToString()));

            await base.ProcessAsync(context, output);
        }

        private async Task<string> RenderCheckboxInputAsync()
        {
            var inputTagHelper = new InputTagHelper(this.htmlGenerator);
            inputTagHelper.For = DataModel;
            inputTagHelper.InputTypeName = "checkbox";
            inputTagHelper.Format = "{0}";
            inputTagHelper.Value = DataModel.Model == null ? string.Empty : DataModel.Model.ToString();
            inputTagHelper.ViewContext = ViewContext;

            TagHelperAttributeList attributes = new TagHelperAttributeList();
            attributes.Add(new TagHelperAttribute("class", new HtmlString("form-check-input")));
            attributes.Add(new TagHelperAttribute("type", new HtmlString(inputTagHelper.InputTypeName)));
            attributes.Add(new TagHelperAttribute("value", new HtmlString(inputTagHelper.Value)));

            return await RenderTagHelperAsync("input", TagMode.SelfClosing, inputTagHelper, attributes);
        }
    }
}
