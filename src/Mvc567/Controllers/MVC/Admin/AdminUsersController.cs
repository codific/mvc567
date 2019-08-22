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

using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc567.Common;
using Mvc567.Services.Infrastructure;
using Mvc567.Common.Attributes;
using Mvc567.DataAccess.Identity;
using Mvc567.Entities.Database;
using Mvc567.Entities.ViewModels.Abstractions.Table;
using Mvc567.Entities.DataTransferObjects.Entities;
using Mvc567.Entities.ViewModels.Mapping;
using Mvc567.Controllers.Abstractions;
using System;
using Mvc567.Entities.DataTransferObjects.ViewModels.AdminViewModels;
using Mvc567.Entities.DataTransferObjects.EmailModels;
using Mvc567.Entities.ViewModels.Abstractions;

namespace Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/users/")]
    [ValidateAdminCookie]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.UsersManagementPolicy)]
    public class AdminUsersController : AbstractEntityController<User, UserDto>
    {
        private readonly IIdentityService identityService;
        private readonly IEmailService emailService;

        public AdminUsersController(IIdentityService identityService, IEntityManager entityManager, IEmailService emailService) : base(entityManager)
        {
            this.identityService = identityService;
            this.emailService = emailService;

            HasDelete = false;
            HasEdit = false;
        }

        protected override void InitNavigationActionsIntoListPage(ref AllEntitiesViewModel model)
        {
            // no global actions
        }

        [HttpGet]
        [Route("{userId}/send-email")]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Send Email", false, 1)]
        public async Task<IActionResult> SendEmailMessageToUser(Guid userId)
        {
            var user = await this.identityService.GetUserByIdAsync(userId);
            if (user != null)
            {
                AdminSendEmailMessageToUserViewModel model = new AdminSendEmailMessageToUserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("{userId}/send-email")]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Send Email", false, 1)]
        public async Task<IActionResult> SendEmailMessageToUser(Guid userId, AdminSendEmailMessageToUserViewModel model)
        {
            var user = await this.identityService.GetUserByIdAsync(userId);
            if (user != null)
            {
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;

                if (ModelState.IsValid)
                {
                    UserMessageEmailModel emailModel = new UserMessageEmailModel
                    {
                        Email = model.Email,
                        Subject = "Administrator Message",
                        GivenName = model.FirstName,
                        Surname = model.LastName,
                        Message = model.Message
                    };

                    var emailResult = await this.emailService.SendEmailAsync("UserMessageEmailView", emailModel);

                    if (emailResult.Success)
                    {
                        TempData["SuccessStatusMessage"] = $"Email to {model.Email} has been sent successfully.";

                        return RedirectToAction(nameof(GetAll));
                    }
                }

                return View(model);
            }

            return NotFound();
        }

        protected override void TableViewActionsInit(ref List<TableRowActionViewModel> actions)
        {
            base.TableViewActionsInit(ref actions);
            actions.Insert(1, TableMapper.CreateAction("Send Email", MaterialDesignIcons.Email, Color.ForestGreen, TableRowActionMethod.Get, $"/{this.controllerRoute}{{0}}/send-email", "[Id]"));
        }
    }
}
