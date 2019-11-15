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

using Newtonsoft.Json;

namespace Codific.Mvc567.Common.Options
{
    public class ConfigurationConstants
    {
        [JsonProperty("GenericValidationRules")]
        public ValidationRules GenericValidationRules { get; set; }

        [JsonProperty("ImageValidationRules")]
        public ValidationRules ImageValidationRules { get; set; }

        [JsonProperty("VideoValidationRules")]
        public ValidationRules VideoValidationRules { get; set; }
    }
}
