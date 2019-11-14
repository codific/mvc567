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

using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
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
    [Route("admin/system/logs/")]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.AccessErrorLogsPolicy)]
    public class AdminLogsController : AbstractEntityController<Log, LogViewModel>
    {
        private readonly ILogService logService;

        public AdminLogsController(IEntityManager entityManager, ILogService logService) : base(entityManager)
        {
            this.logService = logService;

            HasEdit = false;
        }

        protected override void InitNavigationActionsIntoListPage(ref AllEntitiesViewModel model)
        {
            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = "Clean Logs",
                ActionUrl = "/admin/system/logs/clean",
                Icon = MaterialDesignIcons.Delete,
                Method = HttpMethod.Post,
                HasConfirmation = true,
                ConfirmationTitle = "Clean Logs",
                ConfirmationMessage = "Do you really want to clean all system logs?"
            });
        }

        [HttpPost]
        [Route("clean")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CleanLogs()
        {
            bool deletionSuccess = await this.logService.ClearAllLogsAsync();

            if (deletionSuccess)
            {
                TempData["SuccessStatusMessage"] = "Logs have been deleted successfully.";
            }
            else
            {
                TempData["ErrorStatusMessage"] = "Logs have not been deleted. Please check Logs for more information.";
            }

            return RedirectToAction(nameof(GetAll));
        }
    }
}
