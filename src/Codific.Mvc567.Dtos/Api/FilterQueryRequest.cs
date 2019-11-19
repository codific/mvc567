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
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Dtos.Api
{
    public class FilterQueryRequest
    {
        private List<FilterSearchQueryItem> filterQueryStringItems;
        private List<FilterQueryOrderItem> filterQueryOrderItem;

        [FromQuery(Name = "order")]
        public string OrderBy { get; set; }

        [FromQuery(Name = "page-size")]
        public int? PageSize { get; set; }

        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        [FromQuery(Name = "query")]
        public string SearchQuery { get; set; }

        [FromQuery(Name = "deleted")]
        public bool ShowDeleted { get; set; }

        public bool EmptyQuery
        {
            get
            {
                return
                    (this.OrderBy is null) &&
                    (this.PageSize is null) &&
                    (this.Page is null) &&
                    (this.SearchQuery is null) &&
                    (this.ShowDeleted is false);
            }
        }

        public List<FilterSearchQueryItem> FilterQueryStringItems
        {
            get
            {
                if (this.filterQueryStringItems == null && !string.IsNullOrEmpty(this.SearchQuery))
                {
                    Regex regex = new Regex(@"@(.*?)(=|~=|>=|<=|>|<|!=)(.*?);");
                    MatchCollection matches = regex.Matches(this.SearchQuery);
                    if (matches.Count > 0)
                    {
                        this.filterQueryStringItems = new List<FilterSearchQueryItem>();
                        foreach (Match match in matches)
                        {
                            var queryItem = new FilterSearchQueryItem(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                            this.filterQueryStringItems.Add(queryItem);
                        }
                    }
                }

                return this.filterQueryStringItems;
            }
        }

        public List<FilterQueryOrderItem> FilterQueryOrderItems
        {
            get
            {
                if (this.filterQueryOrderItem == null && !string.IsNullOrEmpty(this.OrderBy))
                {
                    Regex regex = new Regex(@"(.*?)(|:a|:d);");
                    MatchCollection matches = regex.Matches(this.OrderBy);
                    if (matches.Count > 0)
                    {
                        this.filterQueryOrderItem = new List<FilterQueryOrderItem>();
                        foreach (Match match in matches)
                        {
                            var queryItem = new FilterQueryOrderItem(match.Groups[1].Value, match.Groups[2].Value.Replace(":", string.Empty));
                            this.filterQueryOrderItem.Add(queryItem);
                        }
                    }
                }

                return this.filterQueryOrderItem;
            }
        }
    }
}
