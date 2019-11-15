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
using Codific.Mvc567.DataAccess.Abstraction;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("database-tables-select", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DatabaseTablesSelectTagHelper : TagHelper
    {
        private readonly IUnitOfWork uow;

        public DatabaseTablesSelectTagHelper(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string selectTag = RenderSelectTag();
            output.Content.SetHtmlContent(new HtmlString(selectTag));
        }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            string selectTag = RenderSelectTag();
            output.Content.SetHtmlContent(new HtmlString(selectTag));
            return base.ProcessAsync(context, output);
        }

        [HtmlAttributeName("selected-value")]
        public string SelectedValue { get; set; }

        [HtmlAttributeName("has-empty")]
        public bool HasEmpty { get; set; }

        private string RenderSelectTag()
        {
            StringBuilder optionsStringBuilder = new StringBuilder();
            var databaseTables = this.uow.DatabaseTables;
            if (HasEmpty)
            {
                optionsStringBuilder.Append($"<option value=\"\"> - </option>");
            }

            foreach (var table in databaseTables)
            {
                string selectedAttribute = string.Empty;
                if (table.Key == SelectedValue)
                {
                    selectedAttribute = "selected ";
                }
                optionsStringBuilder.Append($"<option value=\"{table.Key}\" {selectedAttribute}>{table.Key}</option>");
            }
            return optionsStringBuilder.ToString();
        }
    }
}
