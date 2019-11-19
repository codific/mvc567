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

using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Services.Abstractions;
using Codific.Mvc567.Services.Validators;
using Codific.Mvc567.Services.Validators.Results;
using Codific.Mvc567.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.API
{
    [Route("api/upload")]
    [Authorize(Policy = Policies.AuthorizedUploadPolicy)]
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
        [ProducesResponseType(typeof(FileViewModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> File([FromForm]IFormCollection formCollection)
        {
            var formFile = this.GetFormFileFromFormCollection(formCollection);
            var validationResult = this.validationProvider.ValidateFormFile(formFile);
            if (validationResult.Success)
            {
                var file = await this.fileSystemService.UploadFileAsync<FileViewModel>(formFile);
                if (!(file is null))
                {
                    return this.Ok(file);
                }
            }

            return this.StatusCode(StatusCodes.Status415UnsupportedMediaType, validationResult);
        }

        [HttpPost]
        [Route("image")]
        public IActionResult Image([FromForm]IFormCollection formCollection)
        {
            var result = this.validationProvider.ValidateFormImageFile(this.GetFormFileFromFormCollection(formCollection));

            return this.Json(result);
        }

        [HttpPost]
        [Route("video")]
        public IActionResult Video([FromForm]IFormCollection formCollection)
        {
            var result = this.validationProvider.ValidateFormVideoFile(this.GetFormFileFromFormCollection(formCollection));

            return this.Json(result);
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
