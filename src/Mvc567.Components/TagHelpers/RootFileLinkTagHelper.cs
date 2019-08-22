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
using Microsoft.AspNetCore.Razor.TagHelpers;
using Mvc567.Common;
using Mvc567.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("root-file-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class RootFileLinkTagHelper : TagHelper
    {
        private readonly IFileSystemService fileSystemService;

        public RootFileLinkTagHelper(IFileSystemService fileSystemService)
        {
            this.fileSystemService = fileSystemService;
        }

        [HtmlAttributeName("id")]
        public string FileId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            RenderTag(ref output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            RenderTag(ref output);
            return base.ProcessAsync(context, output);
        }

        public void RenderTag(ref TagHelperOutput output)
        {
            Guid parsedFildId = Guid.Empty;
            if (!string.IsNullOrEmpty(FileId) && Guid.TryParse(FileId, out parsedFildId))
            {
                var fileEntity = fileSystemService.GetFileById(parsedFildId);
                string rootHref = "/admin/roots/";
                if (fileEntity != null && fileEntity.Path.Contains(Constants.PrivateRootFolderName))
                {
                    rootHref += ("private/file/" + fileEntity.Path.Replace("\\", "/").Replace(Constants.PrivateRootFolderName, string.Empty));
                }
                else if (fileEntity != null)
                {
                    rootHref += ("public/file/" + fileEntity.Path.Replace("\\", "/"));
                }

                output.Attributes.Add(new TagHelperAttribute("href", rootHref));
                output.Attributes.Add(new TagHelperAttribute("title", "File"));
                output.Attributes.Add(new TagHelperAttribute("target", "_blank"));
                output.Content.SetHtmlContent(new HtmlString("File"));
            }
            else
            {
                output.Attributes.Add(new TagHelperAttribute("href", "#"));
                output.Attributes.Add(new TagHelperAttribute("title", "No File Available"));
                output.Content.SetHtmlContent(new HtmlString("No File Available"));
            }
        }
    }
}
