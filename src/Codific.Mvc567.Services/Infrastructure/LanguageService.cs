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

using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Entities.DataTransferObjects.Entities;
using Codific.Mvc567.Services.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class LanguageService : AbstractService, ILanguageService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public LanguageService(IUnitOfWork uow, IMapper mapper, IHostingEnvironment hostingEnvironment) : base(uow, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string[] GetAllLanguageCodes()
        {
            try
            {
                var languages = this.standardRepository.GetAll<Language>();
                string[] languageCodes = languages.Select(x => x.Code).ToArray();

                return languageCodes;
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(GetAllLanguageCodesAsync));
                return null;
            }
        }

        public async Task<string[]> GetAllLanguageCodesAsync()
        {
            try
            {
                var languages = await this.standardRepository.GetAllAsync<Language>();
                string[] languageCodes = languages.Select(x => x.Code).ToArray();

                return languageCodes;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetAllLanguageCodesAsync));
                return null;
            }
        }

        public LanguageDto GetDefaultLanguage()
        {
            try
            {
                var languageEntity = this.standardRepository.Query<Language>(x => x.IsDefault).FirstOrDefault();
                var mappedLanguage = this.mapper.Map<LanguageDto>(languageEntity);

                return mappedLanguage;
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(GetAllLanguageCodesAsync));
                return null;
            }
        }

        public async Task<LanguageDto> GetDefaultLanguageAsync()
        {
            try
            {
                var languageEntity = (await this.standardRepository.QueryAsync<Language>(x => x.IsDefault)).FirstOrDefault();
                var mappedLanguage = this.mapper.Map<LanguageDto>(languageEntity);

                return mappedLanguage;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetAllLanguageCodesAsync));
                return null;
            }
        }

        public async Task<bool> GenerateLanguageTranslationFileAsync(Guid languageId)
        {
            try
            {
                var language = await this.standardRepository.GetAsync<Language>(languageId);
                if (language != null)
                {
                    var translationsEntities = await this.standardRepository.QueryAsync<TranslationValue>(
                        x => x.LanguageId == languageId,
                        x => x.OrderBy(y => y.TranslationKey.Key),
                        x => x.Include(y => y.TranslationKey));

                    Dictionary<string, string> translationDictionary = translationsEntities.ToDictionary(x => x.TranslationKey.Key, x => x.Value);

                    if (translationDictionary != null && translationDictionary.Count > 0)
                    {
                        string jsonContent = JsonConvert.SerializeObject(translationDictionary);

                        string languagesDirectory = this.hostingEnvironment.GetLanguagesDirectory();
                        string fileName = $"{language.Code.ToLower()}.json";
                        string saveFilePath = Path.Combine(languagesDirectory, fileName);

                        language.LastTranslationFileGeneration = DateTime.Now;
                        language.TranslationFileUrl = $"{Constants.LanguagesFolderName}/{fileName}";

                        this.standardRepository.Update<Language>(language);

                        await this.uow.SaveChangesAsync();

                        System.IO.File.WriteAllText(saveFilePath, jsonContent);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GenerateLanguageTranslationFileAsync));
                return false;
            }
        }

        public string TranslateKey(string key, string languageCode)
        {
            try
            {
                string value = this.standardRepository.Query<TranslationValue>(x => x.Language.Code.ToLower() == languageCode.ToLower() && x.TranslationKey.Key == key).FirstOrDefault()?.Value;
                return string.IsNullOrEmpty(value) ? key : value;
            }
            catch (Exception)
            {
                return key;
            }
        }
    }
}
