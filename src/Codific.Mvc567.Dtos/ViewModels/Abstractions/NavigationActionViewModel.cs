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
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions
{
    public class NavigationActionViewModel
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string ActionUrl { get; set; }

        public bool SeparatePage { get; set; }

        public HttpMethod Method { get; set; } = HttpMethod.Get;

        public Dictionary<string, string> Parameters { get; set; }

        public bool HasConfirmation { get; set; }

        public string ConfirmationTitle { get; set; }

        public string ConfirmationMessage { get; set; }
    }
}
