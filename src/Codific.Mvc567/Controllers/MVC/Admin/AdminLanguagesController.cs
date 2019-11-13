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
using System.Drawing;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Entities.ViewModels.Mapping;
using Codific.Mvc567.Services.Abstractions;
using Codific.Mvc567.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/languages/")]
    [ValidateAdminCookie]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.LanguagesManagementPolicy)]
    public class AdminLanguagesController : AbstractEntityController<Language, LanguageViewModel>
    {
        private readonly ILanguageService languageService;

        public AdminLanguagesController(IEntityManager entityManager, ILanguageService languageService) : base(entityManager)
        {
            this.languageService = languageService;
        }

        protected override void InitNavigationActionsIntoListPage(ref AllEntitiesViewModel model)
        {
            base.InitNavigationActionsIntoListPage(ref model);

            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = "Translations",
                ActionUrl = "/admin/languages/translations/all",
                Icon = MaterialDesignIcons.Translate,
            });

            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = "Keys",
                ActionUrl = "/admin/languages/translations/keys/all",
                Icon = MaterialDesignIcons.CodeBraces,
            });
        }

        protected override void TableViewActionsInit(ref List<TableRowActionViewModel> actions)
        {
            base.TableViewActionsInit(ref actions);
            actions.Insert(1, TableMapper.CreateAction(
                                            "Translations",
                                            MaterialDesignIcons.Translate,
                                            Color.ForestGreen,
                                            TableRowActionMethod.Get,
                                            $"/{this.controllerRoute}translations/all?q={{0}}",
                                            "[Id]"));
            actions.Insert(2, TableMapper.CreateAction(
                                            "Generate Translation Json", 
                                            MaterialDesignIcons.Json, 
                                            Color.DimGray, 
                                            TableRowActionMethod.Post, 
                                            $"/{this.controllerRoute}{{0}}/generate-translation-file", 
                                            "[Id]"));
        }

        [HttpPost]
        [Route("{languageId}/generate-translation-file")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateTranslationFile(Guid languageId)
        {
            bool generationSuccess = await this.languageService.GenerateLanguageTranslationFileAsync(languageId);

            if (generationSuccess)
            {
                TempData["SuccessStatusMessage"] = "Translation File has been generated successfully.";
            }
            else
            {
                TempData["ErrorStatusMessage"] = "Translation File has not been generated. Please check Logs for more information.";
            }

            return RedirectToAction(nameof(GetAll));
        }
    }
}
