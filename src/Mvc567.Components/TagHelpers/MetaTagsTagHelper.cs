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
using Microsoft.Extensions.Configuration;
using Mvc567.Common.Options;
using System;
using System.Threading.Tasks;

namespace Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("meta-tags", TagStructure = TagStructure.Unspecified)]
    public class MetaTagsTagHelper : AbstractPartialBaseTagHelper
    {
        private readonly IConfiguration configuration;

        public MetaTagsTagHelper(IHtmlHelper htmlHelper, IConfiguration configuration) : base(htmlHelper)
        {
            this.configuration = configuration;
            this.partialPath = "_MetaTagsComponent";
        }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [HtmlAttributeName("image")]
        public string Image { get; set; }

        [HtmlAttributeName("keywords")]
        public string Keywords { get; set; }

        protected override void ValidateTagHelperAttributesExistance()
        {
            if (string.IsNullOrEmpty(this.partialPath))
            {
                throw new NullReferenceException("Partial base tag helper does not have partial.");
            }
        }

        protected override void InnerProcess(ref TagHelperContext context, ref TagHelperOutput output)
        {
            ValidateTagHelperAttributesExistance();

            output.TagName = this.outputTagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            (this.htmlHelper as IViewContextAware).Contextualize(ViewContext);

            string baseUrl = $"{ViewContext.HttpContext.Request.Scheme}://{ViewContext.HttpContext.Request.Host}";
            string queryString = ViewContext.HttpContext.Request.QueryString.HasValue ? ViewContext.HttpContext.Request.QueryString.Value : string.Empty;
            string currentUrl = $"{baseUrl}{ViewContext.HttpContext.Request.Path}{queryString}";

            MetaTagsModel model = new MetaTagsModel();
            model.SetTitle(Title);
            model.SetDescription(Description);
            if (!string.IsNullOrWhiteSpace(Image))
            {
                model.SetImage($"{baseUrl}/{Image}");
            }
            model.Keywords = Keywords;
            model.OpenGraphUrl = currentUrl;
            model.Canonical = currentUrl;

            model.OpenGraphSiteName = this.configuration?["MetaTags:OpenGraphSiteName"];
            model.OpenGraphType = this.configuration?["MetaTags:OpenGraphType"];
            model.FacebookAppId = this.configuration?["MetaTags:FacebookAppId"];
            model.TwitterCard = this.configuration?["MetaTags:TwitterCard"];
            model.TwitterCreator = this.configuration?["MetaTags:TwitterCreator"];
            model.TwitterSite = this.configuration?["MetaTags:TwitterSite"];

            Task<IHtmlContent> htmlContentTask = this.htmlHelper.PartialAsync(this.partialPath, model);
            htmlContentTask.Wait();
            output.Content.SetHtmlContent(htmlContentTask.Result);
        }
    }
}
