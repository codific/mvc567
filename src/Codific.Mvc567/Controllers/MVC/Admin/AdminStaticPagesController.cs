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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Entities.DataTransferObjects.Entities;
using Codific.Mvc567.Services.Infrastructure;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/static-pages/")]
    [ValidateAdminCookie]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.StaticPageManagementPolicy)]
    public class AdminStaticPagesController : AbstractEntityController<StaticPage, StaticPageDto>
    {
        public AdminStaticPagesController(IEntityManager entityManager) : base(entityManager)
        {
            
        }

        [Breadcrumb("Static Pages", true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        public override async Task<IActionResult> Create()
        {
            StaticPageDto model = new StaticPageDto();

            return View(model);
        }

        [Breadcrumb("Static Pages", true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(StaticPageDto model)
        {
            if (ModelState.IsValid)
            {
                var createdPageId = await this.entityManager.CreateEntityAsync<StaticPage, StaticPageDto>(model);
                if (createdPageId.HasValue)
                {
                    return RedirectToAction(nameof(Details), this.controllerName, new { id = createdPageId.Value });
                }
            }

            return View(model);
        }

        [Breadcrumb("Static Pages", true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public override async Task<IActionResult> Edit(Guid id)
        {
            StaticPageDto model = await this.entityManager.GetEntityAsync<StaticPage, StaticPageDto>(id);

            return View(model);
        }

        [Breadcrumb("Static Pages", true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Edit(Guid id, StaticPageDto model)
        {
            if (ModelState.IsValid)
            {
                var editedPageId = await this.entityManager.ModifyEntityAsync<StaticPage, StaticPageDto>(id, model);
                if (editedPageId.HasValue)
                {
                    return RedirectToAction(nameof(Details), this.controllerName, new { id = editedPageId.Value });
                }
            }

            return View(model);
        }
    }
}
