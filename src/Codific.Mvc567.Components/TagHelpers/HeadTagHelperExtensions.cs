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

using System;
using System.Text;
using Codific.Mvc567.Components.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Codific.Mvc567.Components
{
    public static class HeadTagHelperExtensions
    {
        public static void AppendIntoTheHead(this ViewContext viewContext, string headLine)
        {
            if (viewContext.ViewData[typeof(HeadTagHelper).FullName ?? throw new InvalidOperationException()] == null)
            {
                viewContext.ViewData[typeof(HeadTagHelper).FullName ?? throw new InvalidOperationException()] = new StringBuilder();
            }

            ((StringBuilder)viewContext.ViewData[typeof(HeadTagHelper).FullName ?? throw new InvalidOperationException()]).AppendLine(headLine);
        }
    }
}