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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Mvc567.Components.TagHelpers
{
    public abstract class AbstractPartialBaseTagHelper : TagHelper
    {
        protected readonly IHtmlHelper htmlHelper;
        protected string partialPath;
        protected string outputTagName = string.Empty;

        public AbstractPartialBaseTagHelper(IHtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression DataModel { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            InnerProcess(ref context, ref output);

            return base.ProcessAsync(context, output);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            InnerProcess(ref context, ref output);
        }

        protected virtual void ValidateTagHelperAttributesExistance()
        {
            if (DataModel == null)
            {
                throw new NullReferenceException("Partial base tag helper does not have model.");
            }

            if (string.IsNullOrEmpty(this.partialPath))
            {
                throw new NullReferenceException("Partial base tag helper does not have partial.");
            }
        }

        protected virtual void InnerProcess(ref TagHelperContext context, ref TagHelperOutput output)
        {
            ValidateTagHelperAttributesExistance();
            
            output.TagName = this.outputTagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add(new TagHelperAttribute("class", new HtmlString("p-base-group")));
            (this.htmlHelper as IViewContextAware).Contextualize(ViewContext);
            Task<IHtmlContent> htmlContentTask = this.htmlHelper.PartialAsync(this.partialPath, DataModel);
            htmlContentTask.Wait();
            output.Content.SetHtmlContent(htmlContentTask.Result);
        }
    }
}
