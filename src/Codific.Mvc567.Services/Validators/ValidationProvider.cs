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

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Codific.Mvc567.Common.Enums;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Common.Options;
using Codific.Mvc567.Services.Validators.Handlers;
using Codific.Mvc567.Services.Validators.Results;
using Newtonsoft.Json;
using Codific.Mvc567.CommonCore;

namespace Codific.Mvc567.Services.Validators
{
    public class ValidationProvider : IValidationProvider
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ConfigurationConstants configurationConstants;

        public ValidationProvider(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            string configPath = Path.Combine(this.hostingEnvironment.GetPrivateRoot(), "config.json");
            this.configurationConstants = JsonConvert.DeserializeObject<ConfigurationConstants>(System.IO.File.ReadAllText(configPath));
        }

        public ValidationResult ValidateFormFile(IFormFile formFile, List<FileExtensions> customFileExtensions = null, List<string> customMimeTypes = null, long customMaxFileSize = 0)
        {
            List<FileExtensions> allowedFileExtensions = customFileExtensions == null ? this.configurationConstants.GenericValidationRules.AllowedFormatExtensionsAsEnum : customFileExtensions;
            List<string> allowedMimeTypes = customMimeTypes == null ? this.configurationConstants.GenericValidationRules.AllowedMimeTypes : customMimeTypes;
            long maxAllowedFileSize = customMaxFileSize == 0 ? this.configurationConstants.GenericValidationRules.MaxAllowedFileSize : customMaxFileSize;

            var startupHandler = new StartupHandler<IFormFile>();
            var fileExtensionHandler = new FileExtensionHandler(allowedFileExtensions);
            var fileMimeTypesHandler = new FileMimeTypesHandler(allowedMimeTypes);
            var fileSizeHandler = new FileSizeHandler(maxAllowedFileSize);

            startupHandler
                .SetNext(fileExtensionHandler)
                .SetNext(fileMimeTypesHandler)
                .SetNext(fileSizeHandler);

            ValidationResult validationResult = new ValidationResult();
            string resultMessage = string.Empty;
            validationResult.Success = startupHandler.Handle(formFile, out resultMessage) != null;
            validationResult.Message = resultMessage;
            if (validationResult.Success)
            {
                validationResult.Message = "File is valid.";
            }

            return validationResult;
        }

        public ValidationResult ValidateFormImageFile(IFormFile formFile, List<FileExtensions> customFileExtensions = null, List<string> customMimeTypes = null, long customMaxFileSize = 0)
        {
            List<FileExtensions> allowedFileExtensions = customFileExtensions == null ? this.configurationConstants.ImageValidationRules.AllowedFormatExtensionsAsEnum : customFileExtensions;
            List<string> allowedMimeTypes = customMimeTypes == null ? this.configurationConstants.ImageValidationRules.AllowedMimeTypes : customMimeTypes;
            long maxAllowedFileSize = customMaxFileSize == 0 ? this.configurationConstants.ImageValidationRules.MaxAllowedFileSize : customMaxFileSize;

            var startupHandler = new StartupHandler<IFormFile>();
            var fileExtensionHandler = new FileExtensionHandler(allowedFileExtensions);
            var fileMimeTypesHandler = new FileMimeTypesHandler(allowedMimeTypes);
            var fileSizeHandler = new FileSizeHandler(maxAllowedFileSize);
            var imageBoxingHandler = new ImageBoxingHandler();

            startupHandler
                .SetNext(fileExtensionHandler)
                .SetNext(fileMimeTypesHandler)
                .SetNext(fileSizeHandler)
                .SetNext(imageBoxingHandler);

            ValidationResult validationResult = new ValidationResult();
            string resultMessage = string.Empty;
            validationResult.Success = startupHandler.Handle(formFile, out resultMessage) != null;
            validationResult.Message = resultMessage;
            if (validationResult.Success)
            {
                validationResult.Message = "Image file is valid.";
            }

            return validationResult;
        }

        public ValidationResult ValidateFormVideoFile(IFormFile formFile, List<FileExtensions> customFileExtensions = null, List<string> customMimeTypes = null, long customMaxFileSize = 0)
        {
            List<FileExtensions> allowedFileExtensions = customFileExtensions == null ? this.configurationConstants.VideoValidationRules.AllowedFormatExtensionsAsEnum : customFileExtensions;
            List<string> allowedMimeTypes = customMimeTypes == null ? this.configurationConstants.VideoValidationRules.AllowedMimeTypes : customMimeTypes;
            long maxAllowedFileSize = customMaxFileSize == 0 ? this.configurationConstants.VideoValidationRules.MaxAllowedFileSize : customMaxFileSize;

            var startupHandler = new StartupHandler<IFormFile>();
            var fileExtensionHandler = new FileExtensionHandler(allowedFileExtensions);
            var fileMimeTypesHandler = new FileMimeTypesHandler(allowedMimeTypes);
            var fileSizeHandler = new FileSizeHandler(maxAllowedFileSize);

            startupHandler
                .SetNext(fileExtensionHandler)
                .SetNext(fileMimeTypesHandler)
                .SetNext(fileSizeHandler);

            ValidationResult validationResult = new ValidationResult();
            string resultMessage = string.Empty;
            validationResult.Success = startupHandler.Handle(formFile, out resultMessage) != null;
            validationResult.Message = resultMessage;
            if (validationResult.Success)
            {
                validationResult.Message = "Video file is valid.";
            }

            return validationResult;
        }
    }
}
