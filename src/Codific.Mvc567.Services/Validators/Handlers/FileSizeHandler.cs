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
using global::Microsoft.AspNetCore.Http;

namespace Codific.Mvc567.Services.Validators.Handlers
{
    internal class FileSizeHandler : AbstractHandler<IFormFile>
    {
        private readonly long maxAllowedFileSize;

        public FileSizeHandler(long maxAllowedFileSize)
        {
            if (maxAllowedFileSize == 0)
            {
                throw new NullReferenceException("Allowed file size must be greater than 0.");
            }

            this.maxAllowedFileSize = maxAllowedFileSize;
        }

        protected override string HandleProcessAction()
        {
            string resultMessage = string.Empty;
            if (this.RequestObject.Length == 0)
            {
                this.RequestObject = null;
                resultMessage = $"File size must be greater than 0 bytes. ";
            }
            else if (this.RequestObject.Length > this.maxAllowedFileSize)
            {
                this.RequestObject = null;
                resultMessage = $"File size exceeds allowed {this.maxAllowedFileSize} bytes. ";
            }

            return resultMessage;
        }
    }
}