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

using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Dtos.Entities;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("t", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TranslationTagHelper : TagHelper
    {
        private readonly ILanguageService languageService;
        public TranslationTagHelper(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("element")]
        public string Element { get; set; }

        [HtmlAttributeName("language")]
        public string Language { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ProcessOutput(ref output);

            base.Process(context, output);
        }

        private void ProcessOutput(ref TagHelperOutput output)
        {
            output.TagName = string.IsNullOrEmpty(Element) ? "span" : Element;
            output.TagMode = TagMode.StartTagAndEndTag;

            string languageCode = Language;
            if (string.IsNullOrEmpty(Language))
            {
                languageCode = ViewContext.HttpContext.GetLanguageCode();
                if (string.IsNullOrEmpty(languageCode))
                {
                    languageCode = this.languageService.GetDefaultLanguage<SimpleLanguageDto>()?.Code?.ToLower();
                }
            }

            string key = output.GetChildContentAsync().Result.GetContent();
            string translation = this.languageService.TranslateKey(key, languageCode);
            output.Content.Clear();
            output.Content.AppendHtml(new HtmlString(translation));
        }
    }
}
