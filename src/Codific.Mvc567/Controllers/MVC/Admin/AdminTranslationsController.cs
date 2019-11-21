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

using System;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Codific.Mvc567.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/languages/translations/")]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.LanguagesManagementPolicy)]
    public class AdminTranslationsController : AbstractEntityController<TranslationValue, TranslationValueViewModel>
    {
        public AdminTranslationsController(IEntityManager entityManager)
            : base(entityManager)
        {
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbPageTitlePlaceholder, false, 1)]
        public override Task<IActionResult> GetAll([FromQuery(Name = "p")] int page = 1, [FromQuery(Name = "q")] string query = null, [FromQuery(Name = "d")]bool showDeleted = false)
        {
            return base.GetAll(page, query, showDeleted);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Create", false, 2)]
        public override Task<IActionResult> CreateGet(TranslationValueViewModel model = null)
        {
            return base.CreateGet(model);
        }

        [Breadcrumb("Languages", true, 0, "GetAll", "AdminLanguages")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 1, nameof(GetAll))]
        [Breadcrumb("Create", false, 2)]
        public override Task<IActionResult> CreatePost(TranslationValueViewModel model)
        {
            return base.CreatePost(model);
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
        public override Task<IActionResult> Edit(Guid id, TranslationValueViewModel model)
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
