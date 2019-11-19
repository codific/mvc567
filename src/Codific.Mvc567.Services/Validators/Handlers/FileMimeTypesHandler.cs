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
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Codific.Mvc567.Services.Validators.Handlers
{
    internal class FileMimeTypesHandler : AbstractHandler<IFormFile>
    {
        private readonly List<string> allowedMimeTypes;

        public FileMimeTypesHandler(List<string> allowedMimeTypes)
        {
            if (allowedMimeTypes == null || allowedMimeTypes.Count == 0)
            {
                throw new NullReferenceException("Allowed mime types list must be valid list with at least 1 element in.");
            }

            this.allowedMimeTypes = allowedMimeTypes;
        }

        protected override string HandleProcessAction()
        {
            bool isFileValid = false;
            foreach (var mimeType in this.allowedMimeTypes)
            {
                if (this.RequestObject.ContentType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                {
                    isFileValid = true;
                    break;
                }
            }

            string resultMessage = string.Empty;
            if (!isFileValid)
            {
                this.RequestObject = null;
                resultMessage = "File is not image type. ";
            }

            return resultMessage;
        }
    }
}