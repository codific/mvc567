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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.Services.Infrastructure;
using System.Threading.Tasks;

namespace Codific.Mvc567.Controllers.MVC
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class StaticPageController : AbstractController
    {
        private readonly IStaticPageService staticPageService;

        public StaticPageController(
            IConfiguration configuration,
            IEmailService emailService,
            ILanguageService languageService,
            IStaticPageService staticPageService)
            : base(configuration, emailService, languageService)
        {
            this.staticPageService = staticPageService;
        }

        [HttpGet]
        [Route(Constants.ControllerStaticPageRoute)]
        [Route(Constants.LanguageControllerStaticPageRoute)]
        public async Task<IActionResult> PageAction(string route)
        {
            string languageCode = this.HttpContext.GetLanguageCode();
            var model = await this.staticPageService.GetPageByRouteAsync(route, languageCode);
            if (model != null)
            {
                return View(model);
            }

            return NotFound();
        }
    }
}
