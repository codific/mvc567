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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc567.Common.Attributes;
using Mvc567.Controllers.Abstractions;
using Mvc567.DataAccess.Identity;
using Mvc567.Entities.Database;
using Mvc567.Entities.DataTransferObjects.Entities;
using Mvc567.Services.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/languages/translations/keys/")]
    [ValidateAdminCookie]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.LanguagesManagementPolicy)]
    public class AdminTranslationKeysController : AbstractEntityController<TranslationKey, TranslationKeyDto>
    {
        public AdminTranslationKeysController(IEntityManager entityManager) : base(entityManager)
        {

        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbPageTitlePlaceholder, false, 1)]
        public override Task<IActionResult> GetAll([FromQuery(Name = "p")] int page = 1, [FromQuery(Name = "q")] string query = null)
        {
            return base.GetAll(page, query);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Create", false, 2)]
        public override Task<IActionResult> Create()
        {
            return base.Create();
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Create", false, 2)]
        public override Task<IActionResult> Create(TranslationKeyDto model)
        {
            return base.Create(model);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Edit", false, 2)]
        public override Task<IActionResult> Edit(Guid id)
        {
            return base.Edit(id);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Edit", false, 2)]
        public override Task<IActionResult> Edit(Guid id, TranslationKeyDto model)
        {
            return base.Edit(id, model);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Details", false, 2)]
        public override Task<IActionResult> Details(Guid id)
        {
            return base.Details(id);
        }
    }
}
