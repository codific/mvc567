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

using System.Collections.Generic;
using System.Drawing;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions.Table
{
    public class TableRowActionViewModel
    {
        public TableRowActionViewModel(
            string title,
            string icon,
            Color color,
            string urlStringFormat,
            List<string> rawParameters,
            TableRowActionMethod method = TableRowActionMethod.Get,
            bool isToBeOpenedInNewTab = false)
        {
            this.Title = title;
            this.Icon = icon;
            this.Color = color;
            this.UrlStringFormat = urlStringFormat;
            this.RawParameters = rawParameters;
            this.Method = method;
            this.IsToBeOpenedInNewTab = isToBeOpenedInNewTab;
        }

        public string Title { get; set; }

        public TableRowActionMethod Method { get; set; }

        public bool HasConfirmation { get; set; }

        public string Icon { get; set; }

        public Color Color { get; set; }

        public string ConfirmationTitle { get; set; }

        public string ConfirmationMessage { get; set; }

        public bool IsToBeOpenedInNewTab { get; set; }

        public string Url
        {
            get
            {
                return string.Format(this.UrlStringFormat, this.Parameters.ToArray());
            }
        }

        public string UrlStringFormat { get; set; }

        public List<string> RawParameters { get; set; }

        public List<string> Parameters { get; set; }

        public void SetConfirmation(string confirmationTitle, string confirmationMessage)
        {
            this.HasConfirmation = true;
            this.ConfirmationTitle = confirmationTitle;
            this.ConfirmationMessage = confirmationMessage;
        }
    }
}
