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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc567.Entities.DataTransferObjects.Entities;
using Mvc567.Services.Infrastructure;
using Mvc567.Services.Validators;
using Mvc567.Services.Validators.Results;

namespace Mvc567.Controllers.API
{
    [Route("api/upload")]
    public class UploadController : Controller
    {
        private readonly IValidationProvider validationProvider;
        private readonly IFileSystemService fileSystemService;

        public UploadController(IValidationProvider validationProvider, IFileSystemService fileSystemService)
        {
            this.validationProvider = validationProvider;
            this.fileSystemService = fileSystemService;
        }

        [HttpPost]
        [Route("file")]
        [ProducesResponseType(typeof(ValidationResult), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(FileDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> File([FromForm]IFormCollection formCollection)
        {
            var formFile = GetFormFileFromFormCollection(formCollection);
            var validationResult = this.validationProvider.ValidateFormFile(formFile);
            if (validationResult.Success)
            {
                var file = await this.fileSystemService.UploadFileAsync(formFile);
                if (file != null)
                {
                    return Ok(file);
                }
            }

            return StatusCode(StatusCodes.Status415UnsupportedMediaType, validationResult);
        }

        [HttpPost]
        [Route("image")]
        public async Task<IActionResult> Image([FromForm]IFormCollection formCollection)
        {
            var result = this.validationProvider.ValidateFormImageFile(GetFormFileFromFormCollection(formCollection));

            return Json(result);
        }

        [HttpPost]
        [Route("video")]
        public async Task<IActionResult> Video([FromForm]IFormCollection formCollection)
        {
            var result = this.validationProvider.ValidateFormVideoFile(GetFormFileFromFormCollection(formCollection));

            return Json(result);
        }

        private IFormFile GetFormFileFromFormCollection(IFormCollection formCollection)
        {
            IFormFile formFile = null;
            if (formCollection != null && formCollection.Files.Count > 0)
            {
                formFile = formCollection.Files[0];
            }

            return formFile;
        }
    }
}
