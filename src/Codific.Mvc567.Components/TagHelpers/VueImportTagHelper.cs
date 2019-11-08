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

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("vue-import", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class VueImportTagHelper : PartialTagHelper
    {
        private const string ComponentsPathStringFormat = "Vue/_{0}.Vue";

        public VueImportTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope bufferScope) : base(viewEngine, bufferScope)
        {

        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Name = string.Format(ComponentsPathStringFormat, Name);
            base.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Name = string.Format(ComponentsPathStringFormat, Name);
            return base.ProcessAsync(context, output);
        }
    }
}
