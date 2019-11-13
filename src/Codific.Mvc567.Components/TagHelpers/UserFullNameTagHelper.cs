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

using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codific.Mvc567.Components.TagHelpers
{
    [HtmlTargetElement("user-full-name", TagStructure = TagStructure.WithoutEndTag)]
    public class UserFullNameTagHelper : TagHelper
    {
        private readonly IIdentityService identityService;

        public UserFullNameTagHelper(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            var userEmail = this.ViewContext.HttpContext.User.Identity.Name;

            var user = await this.identityService.GetUserByEmailAsync(userEmail);

            string firstName = string.Empty;
            string lastName = string.Empty;
            if (user != null)
            {
                firstName = user.FirstName;
                lastName = user.LastName;
            }
            output.Content.SetContent($"{firstName} {lastName}");
        }
    }
}
