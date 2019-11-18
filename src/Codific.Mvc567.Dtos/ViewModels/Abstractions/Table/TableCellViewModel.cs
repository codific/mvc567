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
using System.Globalization;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Enums;

namespace Codific.Mvc567.Dtos.ViewModels.Abstractions.Table
{
    public class TableCellViewModel
    {
        private object content = string.Empty;

        public TableCellViewModel(int order, object content, TableCellType type, bool editable, string relatedProperty)
        {
            this.Order = order;
            this.Type = type;
            this.content = content;
            this.Editable = editable;
            this.RelatedProperty = relatedProperty;
        }

        public int Order { get; private set; }

        public TableCellType Type { get; private set; }

        public int NumberDecimalDigits { get; set; }

        public string MoneyCurrency { get; set; }

        public string RelatedProperty { get; set; } = "TestRelatedProperty";

        public bool Editable { get; set; } = true;

        public string Content
        {
            get
            {
                string resultContent = string.Empty;
                switch (this.Type)
                {
                    case TableCellType.Text:
                        resultContent = this.content?.ToString();
                        break;
                    case TableCellType.Date:
                        resultContent = !string.IsNullOrEmpty(this.content?.ToString()) ? ((DateTime)this.content).ToString(Constants.DateFormat) : string.Empty;
                        break;
                    case TableCellType.Time:
                        resultContent = !string.IsNullOrEmpty(this.content?.ToString()) ? ((TimeSpan)this.content).ToString(Constants.TimeFormat) : string.Empty;
                        break;
                    case TableCellType.DateTime:
                        resultContent = !string.IsNullOrEmpty(this.content?.ToString()) ? ((DateTime)this.content).ToString(Constants.DateTimeFormat) : string.Empty;
                        break;
                    case TableCellType.Number:
                        resultContent = string.Format(new NumberFormatInfo() { NumberDecimalDigits = this.NumberDecimalDigits }, "{0:F}", (float)this.content);
                        break;
                    case TableCellType.File:
                        resultContent = this.content?.ToString();
                        break;
                    case TableCellType.Flag:
                        resultContent = this.content.ToString();
                        break;
                    case TableCellType.TextArea:
                        resultContent = this.content?.ToString();
                        break;
                    default:
                        resultContent = this.content?.ToString();
                        break;
                }

                return resultContent;
            }
        }

        public void SetRawContent(object content)
        {
            this.content = content;
        }
    }
}
