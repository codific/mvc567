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

namespace Codific.Mvc567.Common.Options
{
    public class MetaTagsModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Author { get; set; }

        public string Canonical { get; set; }

        public string OpenGraphType { get; set; }

        public string OpenGraphTitle { get; set; }

        public string OpenGraphDescription { get; set; }

        public string OpenGraphImage { get; set; }

        public string OpenGraphUrl { get; set; }

        public string OpenGraphSiteName { get; set; }

        public string FacebookAppId { get; set; }

        public string TwitterCard { get; set; }

        public string TwitterTitle { get; set; }

        public string TwitterDescription { get; set; }

        public string TwitterImage { get; set; }

        public string TwitterSite { get; set; }

        public string TwitterCreator { get; set; }

        public void SetTitle(string title)
        {
            this.Title = title;
            this.OpenGraphTitle = title;
            this.TwitterTitle = title;
        }

        public void SetDescription(string description)
        {
            this.Description = description;
            this.OpenGraphDescription = description;
            this.TwitterDescription = description;
        }

        public void SetImage(string imageUrl)
        {
            this.OpenGraphImage = imageUrl;
            this.TwitterImage = imageUrl;
        }
    }
}
