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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.DataAccess.Abstraction.Entities;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Entities.DataTransferObjects.ServiceResults;
using Codific.Mvc567.Services.Abstractions;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class SEOService : AbstractService, ISEOService
    {
        private readonly IConfiguration configuration;
        private readonly Regex sitemapPatternRegex = new Regex(@"\[(.*?)\]");
        private readonly IHttpContextAccessor httpAccessor;
        private readonly string baseUrl;
        private const string RobotsTxtConfigurationKey = "RobotsTxt";

        public SEOService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpAccessor) : base(uow, mapper)
        {
            this.configuration = configuration;
            this.httpAccessor = httpAccessor;
            this.baseUrl = $"{this.httpAccessor.HttpContext.Request.Scheme}://{this.httpAccessor.HttpContext.Request.Host}";
        }

        public RobotsTxtResult GenerateRobotsTxt()
        {
            try
            {
                StringBuilder robotsOutput = new StringBuilder();
                var robotsTxtArray = this.configuration
                    .GetSection(RobotsTxtConfigurationKey)
                    .AsEnumerable()
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .OrderBy(x => x.Key)
                    .ToArray();

                for (int i = 0; i < robotsTxtArray.Count(); i++)
                {
                    robotsOutput.AppendLine(robotsTxtArray[i].Value);
                }

                var robotsResult = new RobotsTxtResult
                {
                    Content = robotsOutput.ToString()
                };

                return robotsResult;
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(GenerateRobotsTxt));
                return new RobotsTxtResult();
            }
            
        }

        public async Task<SitemapResult> GenerateSitemapAsync()
        {
            var patternsEntities = await this.standardRepository.GetAllAsync<SitemapItemPattern>();
            
            SitemapResult result = new SitemapResult();
            foreach (var pattern in patternsEntities)
            {
                if ((pattern.DomainOnly && this.baseUrl == pattern.Domain) || !pattern.DomainOnly)
                {
                    string domain = string.IsNullOrEmpty(pattern.Domain) ? this.baseUrl : pattern.Domain;
                    if (!pattern.SinglePage)
                    {
                        var petternUrls = GetUrlsByPattern(pattern.Pattern, pattern.RelatedEntity);
                        foreach (var url in petternUrls)
                        {
                            result.Urls.Add(new Url
                            {
                                Location = $"{domain}{url}",
                                Priority = pattern.Priority.ToString(),
                                ChangeFrequency = pattern.ChangeFrequency.ToString()
                            });
                        }
                    }
                    else
                    {
                        result.Urls.Add(new Url
                        {
                            Location = $"{domain}{pattern.Pattern}",
                            Priority = pattern.Priority.ToString(),
                            ChangeFrequency = pattern.ChangeFrequency.ToString()
                        });
                    }
                }
            }

            return result;
        }

        private List<IEntityBase> GetAllEntitiesByTableName(string tableName)
        {
            return this.uow.GetStandardRepository().GetAllByEntityTableName(tableName).ToList();
        }

        private List<string> GetUrlsByPattern(string pattern, string tableName)
        {
            List<string> resultUrls = new List<string>();
            MatchCollection patternMatches = this.sitemapPatternRegex.Matches(pattern);
            if (patternMatches.Count > 0)
            {
                List<string> entityPropertiesMatches = new List<string>();
                for (int i = 0; i < patternMatches.Count; i++)
                {
                    entityPropertiesMatches.Add(patternMatches[0].Value);
                }

                var entities = GetAllEntitiesByTableName(tableName);
                if (entities != null && entities.Count > 0)
                {
                    foreach (var entity in entities)
                    {
                        string currentUrl = pattern;
                        for (int i = 0; i < patternMatches.Count; i++)
                        {
                            string entityValue = entity.GetType().GetProperty(patternMatches[i].Groups[1].Value).GetValue(entity)?.ToString();
                            if (!string.IsNullOrEmpty(entityValue))
                            {
                                currentUrl = currentUrl.Replace(patternMatches[i].Value, entityValue);
                            }
                        }
                        resultUrls.Add(currentUrl);
                    }
                }
            }

            return resultUrls;
        }
    }
}
