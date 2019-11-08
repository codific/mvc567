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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Utilities;

namespace Codific.Mvc567.Components.TagHelpers
{
    public abstract class AbstractFormComponentTagHelper : TagHelper
    {
        protected readonly IHtmlGenerator htmlGenerator;

        public AbstractFormComponentTagHelper(IHtmlGenerator htmlGenerator)
        {
            this.htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression DataModel { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected async Task<string> RenderTagHelperAsync(string tagName, TagMode tagMode, ITagHelper tagHelper, TagHelperAttributeList tagHelperAttributes = null)
        {
            if (tagHelperAttributes == null)
            {
                tagHelperAttributes = new TagHelperAttributeList();
            }

            TagHelperOutput output = new TagHelperOutput(tagName, tagHelperAttributes, (useCachedResult, encoder) =>
            {
                return Task.Run<TagHelperContent>(() => new DefaultTagHelperContent());
            })
            {
                TagMode = tagMode
            };

            TagHelperContext context = new TagHelperContext(tagHelperAttributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            tagHelper.Init(context);
            await tagHelper.ProcessAsync(context, output);

            return output.RenderTag();
        }

        protected async Task<string> RenderValidationTagHelperAsync()
        {
            ValidationMessageTagHelper validationMessageTagHelper = new ValidationMessageTagHelper(this.htmlGenerator);

            validationMessageTagHelper.For = DataModel;
            validationMessageTagHelper.ViewContext = ViewContext;

            string tagString = await RenderTagHelperAsync("span",
                TagMode.StartTagAndEndTag,
                validationMessageTagHelper,
                new TagHelperAttributeList(new[]
                {
                    new TagHelperAttribute("class", new HtmlString("text-danger text-small"))
                }));

            return tagString;
        }
    }
}
