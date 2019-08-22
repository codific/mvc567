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
using Microsoft.Extensions.Configuration;
using Mvc567.Controllers.Abstractions;
using Mvc567.Entities.DataTransferObjects.ServiceResults;
using Mvc567.Services.Infrastructure;

namespace Mvc567.Controllers.MVC
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SEOController : AbstractController
    {
        private readonly ISEOService seoService;

        public SEOController(
            IConfiguration configuration, 
            IEmailService emailService, 
            ILanguageService languageService,
            ISEOService seoService)
            : base(configuration, emailService, languageService)
        {
            this.seoService = seoService;
        }

        [HttpGet]
        [Produces("text/plain")]
        [Route("robots.txt")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Robots()
        {
            RobotsTxtResult robotsTxtResult = this.seoService.GenerateRobotsTxt();

            return Ok(robotsTxtResult.Content);
        }

        [HttpGet]
        [Produces("application/xml")]
        [Route("sitemap.xml")]
        [ProducesResponseType(typeof(SitemapResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Sitemap()
        {
            SitemapResult sitemapResult = await this.seoService.GenerateSitemapAsync();

            return Ok(sitemapResult);
        }
    }
}
