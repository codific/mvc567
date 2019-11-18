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

using System.IO;
using Codific.Mvc567.Common;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Shared
{
    [Route("resources")]
    public class ResourcesController : Controller
    {
        [HttpGet]
        [Route("images/{resourceName}.{contentFormatType}")]
        public IActionResult GetImage(string resourceName, string contentFormatType)
        {
            try
            {
                string memeType = MimeTypes.Image.GetContentType(contentFormatType);
                if (!string.IsNullOrEmpty(memeType))
                {
                    byte[] fileBytes = (byte[])Resources.ResourceManager.GetObject(resourceName);
                    Stream stream = new MemoryStream(fileBytes);
                    return this.File(stream, memeType);
                }
            }
            catch (System.Exception)
            {
                // ignored
            }

            return this.NotFound();
        }

        [HttpGet]
        [Route("fonts/{resourceName}.{contentFormatType}")]
        public IActionResult GetFont(string resourceName, string contentFormatType)
        {
            try
            {
                string memeType = MimeTypes.Font.GetContentType(contentFormatType);
                if (!string.IsNullOrEmpty(memeType))
                {
                    byte[] fileBytes = (byte[])Resources.ResourceManager.GetObject(resourceName.Replace('.', '_').Replace('-', '_') + $"_{contentFormatType}");
                    Stream stream = new MemoryStream(fileBytes);

                    return this.File(stream, memeType);
                }
            }
            catch (System.Exception)
            {
                // ignored
            }

            return this.NotFound();
        }

        [HttpGet]
        [Route("css/{resourceName}.css")]
        public IActionResult GetCss(string resourceName, string contentFormatType)
        {
            try
            {
                string memeType = MimeTypes.Text.Css;
                if (!string.IsNullOrEmpty(memeType))
                {
                    string convertedResourceName = resourceName.Replace('.', '_').Replace('-', '_') + "_css";
                    byte[] fileBytes = (byte[])Resources.ResourceManager.GetObject(convertedResourceName);
                    Stream stream = new MemoryStream(fileBytes);

                    return this.File(stream, memeType);
                }
            }
            catch (System.Exception)
            {
                // ignored
            }

            return this.NotFound();
        }
    }
}
