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
using Microsoft.EntityFrameworkCore;
using Codific.Mvc567.Common;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Entities.DataTransferObjects.Entities;
using Codific.Mvc567.Services.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class StaticPageService : AbstractService, IStaticPageService
    {
        private readonly ILanguageService languageService;

        public StaticPageService(IUnitOfWork uow, IMapper mapper, ILanguageService languageService) : base(uow, mapper)
        {
            this.languageService = languageService;
        }

        public async Task<StaticPageDto> GetPageByRouteAsync(string route, string languageCode)
        {
            try
            {
                var pageLanguage = await GetLanguageByCodeAsync(languageCode);
                var matchedPage = (await this.standardRepository
                    .QueryAsync<StaticPage>(
                        x => x.Route == route && x.Active && x.LanguageId == pageLanguage.Id,
                        null,
                        x => x.Include(y => y.Image)))
                    .FirstOrDefault();
                if (matchedPage != null)
                {
                    return this.mapper.Map<StaticPageDto>(matchedPage);
                }

                return null;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetPageByRouteAsync));
                return null;
            }
        }

        private async Task<Language> GetLanguageByCodeAsync(string languageCode)
        {
            Language language = null;
            if (!string.IsNullOrEmpty(languageCode))
            {
                language = (await this.standardRepository.QueryAsync<Language>(x => x.Code.ToLower() == languageCode.ToLower())).FirstOrDefault();
            }
            else
            {
                var defaultLanguage = await this.languageService.GetDefaultLanguageAsync();
                if (defaultLanguage != null)
                {
                    language = (await this.standardRepository.QueryAsync<Language>(x => x.Code.ToLower() == defaultLanguage.Code.ToLower())).FirstOrDefault();
                }
            }

            return language;
        }
    }
}
