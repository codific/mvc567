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

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mvc567.Entities.DataTransferObjects.Api
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

        public bool EmptyQuery
        {
            get
            {
                return
                    (OrderBy is null) &&
                    (PageSize is null) &&
                    (Page is null) &&
                    (SearchQuery is null);
            }
        }

        public List<FilterSearchQueryItem> FilterQueryStringItems
        {
            get
            {
                if (this.filterQueryStringItems == null && !string.IsNullOrEmpty(SearchQuery))
                {
                    Regex regex = new Regex(@"@(.*?)(=|~=|>=|<=|>|<|!=)(.*?);");
                    MatchCollection matches = regex.Matches(SearchQuery);
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
                if (this.filterQueryOrderItem == null && !string.IsNullOrEmpty(OrderBy))
                {
                    Regex regex = new Regex(@"(.*?)(|:a|:d);");
                    MatchCollection matches = regex.Matches(OrderBy);
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
