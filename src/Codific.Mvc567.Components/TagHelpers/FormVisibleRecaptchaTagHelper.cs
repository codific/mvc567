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

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("form-visible-recaptcha", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormVisibleRecaptchaTagHelper : TagHelper
    {
        private readonly IConfiguration configuration;

        public FormVisibleRecaptchaTagHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add(new TagHelperAttribute("class", $"form-group"));

            StringBuilder outputStringBuilder = new StringBuilder();

            outputStringBuilder.Append($"<div class=\"g-recaptcha\" data-sitekey=\"{this.configuration["GoogleRecaptchaKeys:VisibleRecaptcha:SiteKey"]}\"></div>");
            if (this.ViewContext.ModelState.ContainsKey("ReCaptcha") && this.ViewContext.ModelState["ReCaptcha"].Errors != null && this.ViewContext.ModelState["ReCaptcha"].Errors.Count > 0)
            {
                foreach (var error in this.ViewContext.ModelState["ReCaptcha"].Errors)
                {
                    outputStringBuilder.Append($"<span class=\"text-danger text-small\">{error.ErrorMessage}</span>");
                }
            }

            output.Content.SetHtmlContent(new HtmlString(outputStringBuilder.ToString()));

            this.ViewContext.AppendIntoTheHead("<script src=\"https://www.google.com/recaptcha/api.js\" async defer></script>");

            return base.ProcessAsync(context, output);
        }
    }
}