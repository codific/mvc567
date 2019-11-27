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
using System.Threading.Tasks;
using Codific.Mvc567.Common.Options;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("meta-tags", TagStructure = TagStructure.Unspecified)]
    public class MetaTagsTagHelper : PartialBaseTagHelper
    {
        private readonly IConfiguration configuration;

        public MetaTagsTagHelper(IHtmlHelper htmlHelper, IConfiguration configuration)
            : base(htmlHelper)
        {
            this.configuration = configuration;
            this.PartialPath = "_MetaTagsComponent";
        }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [HtmlAttributeName("image")]
        public string Image { get; set; }

        [HtmlAttributeName("keywords")]
        public string Keywords { get; set; }

        protected override void ValidateTagHelperAttributesExistence()
        {
            if (string.IsNullOrEmpty(this.PartialPath))
            {
                throw new NullReferenceException("Partial base tag helper does not have partial.");
            }
        }

        protected override void InnerProcess(ref TagHelperContext context, ref TagHelperOutput output)
        {
            this.ValidateTagHelperAttributesExistence();

            output.TagName = this.OutputTagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            (this.HtmlHelper as IViewContextAware)?.Contextualize(this.ViewContext);

            string baseUrl = $"{this.ViewContext.HttpContext.Request.Scheme}://{this.ViewContext.HttpContext.Request.Host}";
            string queryString = this.ViewContext.HttpContext.Request.QueryString.HasValue ? this.ViewContext.HttpContext.Request.QueryString.Value : string.Empty;
            string currentUrl = $"{baseUrl}{this.ViewContext.HttpContext.Request.Path}{queryString}";

            MetaTagsModel model = new MetaTagsModel();
            model.SetTitle(this.Title);
            model.SetDescription(this.Description);
            if (!string.IsNullOrWhiteSpace(this.Image))
            {
                model.SetImage($"{baseUrl}/{this.Image}");
            }

            model.Keywords = this.Keywords;
            model.OpenGraphUrl = currentUrl;
            model.Canonical = currentUrl;

            model.OpenGraphSiteName = this.configuration?["MetaTags:OpenGraphSiteName"];
            model.OpenGraphType = this.configuration?["MetaTags:OpenGraphType"];
            model.FacebookAppId = this.configuration?["MetaTags:FacebookAppId"];
            model.TwitterCard = this.configuration?["MetaTags:TwitterCard"];
            model.TwitterCreator = this.configuration?["MetaTags:TwitterCreator"];
            model.TwitterSite = this.configuration?["MetaTags:TwitterSite"];

            Task<IHtmlContent> htmlContentTask = this.HtmlHelper.PartialAsync(this.PartialPath, model);
            htmlContentTask.Wait();
            output.Content.SetHtmlContent(htmlContentTask.Result);
        }
    }
}