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
using Microsoft.AspNetCore.Razor.TagHelpers;
using Codific.Mvc567.Components.TagHelpers.Utilities;
using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("database-checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DatabaseCheckboxTagHelper : TagHelper
    {
        private readonly IStandardRepository standardRepository;

        public DatabaseCheckboxTagHelper(IStandardRepository standardRepository)
        {
            this.standardRepository = standardRepository;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "row m-0");
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
        }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "row m-0");
            string tagContent = RenderTag();
            output.Content.SetHtmlContent(new HtmlString(tagContent));
            return base.ProcessAsync(context, output);
        }

        [HtmlAttributeName("entity-type")]
        public Type DatabaseEntityType { get; set; }

        [HtmlAttributeName("visible-property")]
        public string VisibleProperty { get; set; }

        [HtmlAttributeName("selected-values")]
        public Guid[] SelectedValues { get; set; }

        [HtmlAttributeName("model-name")]
        public string ModelName { get; set; }

        private string RenderTag()
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            var databaseEntities = this.standardRepository.GetAllByType(DatabaseEntityType);
            var databaseEntitiesDictionary = Functions.GetDatabaseEntityDictionary(databaseEntities, VisibleProperty);

            foreach (var entityItem in databaseEntitiesDictionary)
            {
                contentStringBuilder.Append(GetRadioItemHtml(entityItem.Value, entityItem.Key, SelectedValues?.Contains(entityItem.Key)));
            }
            return contentStringBuilder.ToString();
        }

        private string GetRadioItemHtml(string name, Guid value, bool? selected)
        {
            StringBuilder contentStringBuilder = new StringBuilder();
            contentStringBuilder.Append("<div class=\"form-check mr-3 mt-1 mb-2 form-check-flat\">");
            contentStringBuilder.Append("<label class=\"form-check-label\">");
            contentStringBuilder.Append($"<input type=\"checkbox\" class=\"form-check-input\" name=\"{ModelName}[]\" id=\"flatCheckbox-{Guid.NewGuid().ToString()}\" {(selected.HasValue && selected.Value ? "checked=\"checked\"" : string.Empty)} value=\"{value}\"> {name} <i class=\"input-helper\"></i>");
            contentStringBuilder.Append("</label>");
            contentStringBuilder.Append("</div>");

            return contentStringBuilder.ToString();
        }
    }
}
