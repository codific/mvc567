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
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.EmailModels;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Codific.Mvc567.Dtos.ViewModels.AdminViewModels;
using Codific.Mvc567.Dtos.ViewModels.Mapping;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Codific.Mvc567.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/users/")]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    [Authorize(Policy = ApplicationPermissions.UsersManagementPolicy)]
    public class AdminUsersController : AdminEntityController<User, UserViewModel>
    {
        private readonly IIdentityService identityService;
        private readonly IEmailService emailService;
        private readonly IAuthenticationService authenticationService;
        private readonly UserManager<User> userManager;

        public AdminUsersController(
            IIdentityService identityService,
            IEntityManager entityManager,
            IEmailService emailService,
            IAuthenticationService authenticationService,
            UserManager<User> userManager)
            : base(entityManager)
        {
            this.identityService = identityService;
            this.emailService = emailService;
            this.authenticationService = authenticationService;
            this.userManager = userManager;
            this.HasDelete = false;
            this.HasEdit = false;
        }

        [HttpGet]
        [Route("{userId}/send-email")]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Send Email", false, 1)]
        public async Task<IActionResult> SendEmailMessageToUser(Guid userId)
        {
            var user = await this.identityService.GetUserByIdAsync<User>(userId);
            if (user != null)
            {
                AdminSendEmailMessageToUserViewModel model = new AdminSendEmailMessageToUserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                return this.View(model);
            }

            return this.NotFound();
        }

        [HttpPost]
        [Route("{userId}/reset-refresh-token")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ResetUserRefreshToken(Guid userId)
        {
            bool successRefresh = await this.authenticationService.ResetUserRefreshTokensAsync(userId);
            if (successRefresh)
            {
                this.TempData["SuccessStatusMessage"] = "Refresh token has been reset successfully.";
            }
            else
            {
                this.TempData["ErrorStatusMessage"] = "Refresh token reset failed.";
            }

            return this.RedirectToAction(nameof(this.GetAll));
        }

        [HttpPost]
        [Route("{userId}/send-email")]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Send Email", false, 1)]
        public virtual async Task<IActionResult> SendEmailMessageToUser(Guid userId, AdminSendEmailMessageToUserViewModel model)
        {
            var user = await this.identityService.GetUserByIdAsync<User>(userId);
            if (user != null)
            {
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;

                if (this.ModelState.IsValid)
                {
                    UserMessageEmailModel emailModel = new UserMessageEmailModel
                    {
                        Email = model.Email,
                        Subject = "Administrator Message",
                        GivenName = model.FirstName,
                        Surname = model.LastName,
                        Message = model.Message,
                    };

                    var emailResult = await this.emailService.SendEmailAsync("UserMessageEmailView", emailModel);

                    if (emailResult.Success)
                    {
                        this.TempData["SuccessStatusMessage"] = $"Email to {model.Email} has been sent successfully.";

                        return this.RedirectToAction(nameof(this.GetAll));
                    }
                }

                return this.View(model);
            }

            return this.NotFound();
        }

        [HttpPost]
        [Route("{userId}/reset-mfa")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ResetUserMfa(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            await this.userManager.SetTwoFactorEnabledAsync(user, false);
            var successRefresh = await this.userManager.ResetAuthenticatorKeyAsync(user);

            if (successRefresh.Succeeded)
            {
                this.TempData["SuccessStatusMessage"] = "MFA has been reset successfully.";
            }
            else
            {
                this.TempData["ErrorStatusMessage"] = "MFA reset failed.";
            }

            return this.RedirectToAction(nameof(this.GetAll));
        }

        [HttpGet]
        [Route("create-new")]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Create New User", false, 1)]
        public virtual IActionResult CreateNewUser()
        {
            var model = new AdminCreateNewUserViewModel();
            model.RoleType = typeof(Role);

            return this.View(model);
        }

        [HttpPost]
        [Route("create-new")]
        [Breadcrumb("Users", true, 0, nameof(GetAll))]
        [Breadcrumb("Create New User", false, 1)]
        public virtual async Task<IActionResult> CreateNewUser(AdminCreateNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var selectedRole = await this.EntityManager.GetEntityAsync<Role, Role>(model.Role);
                var user = await this.authenticationService.CreateUserAsync<User>(
                    model.Email,
                    StringFunctions.GenerateRandomPassword(),
                    model.FirstName,
                    model.LastName,
                    new string[] { selectedRole.Name });

                if (user != null)
                {
                    var resetToken = await this.identityService.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordUrl = this.Url.Action("ResetPassword", "AdminAccount", new { userId = user.Id, resetToken = resetToken }, this.HttpContext.Request.Scheme);

                    var mailModel = new UserResetPasswordEmailModel
                    {
                        Email = user.Email,
                        Subject = "Password Reset",
                        GivenName = user.FirstName,
                        Surname = user.LastName,
                        resetPasswordUrl = resetPasswordUrl,
                    };
                    var result = await this.emailService.SendEmailAsync("UserResetPasswordEmailView", mailModel);
                    if (result.Success)
                    {
                        this.TempData["SuccessStatusMessage"] =
                            $"User has been created successfully.";
                    }
                    else
                    {
                        this.TempData["ErrorStatusMessage"] = $"An error occured while creating user.";
                    }

                    return this.RedirectToAction(nameof(this.GetAll));
                }
            }

            return this.View(model);
        }

        [HttpGet]
        [Route("{id}/edit-user")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public virtual async Task<IActionResult> EditUser(Guid id)
        {
            ICreateEditEntityDto model = (ICreateEditEntityDto)(await this.EntityManager.GetEntityAsync<User, AdminEditUserViewModel>(id));
            model.Area = "Admin";
            model.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            model.Action = "EditUser";
            model.EntityName = "User";
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = "Users";

            return this.View("AbstractViews/Edit", model);
        }

        [HttpPost]
        [Route("{id}/edit-user")]
        [ValidateAntiForgeryToken]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public async Task<IActionResult> EditUser(Guid id, AdminEditUserViewModel model)
        {
            ICreateEditEntityDto castedModel = (ICreateEditEntityDto)model;
            castedModel.Area = "Admin";
            castedModel.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            castedModel.Action = "Edit";
            castedModel.EntityName = "User";
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = "Users";
            this.ModelState.Remove("Area");
            this.ModelState.Remove("Controller");
            this.ModelState.Remove("Action");
            this.ModelState.Remove("EntityName");

            if (this.ModelState.IsValid)
            {
                if (await this.identityService.EditUserAsync(model.Id, model.Email, model.FirstName, model.LastName))
                {
                    this.TempData["SuccessStatusMessage"] = "User data has been successfully edited.";
                    return this.RedirectToAction("GetAll", this.ControllerName);
                }
            }

            return this.View("AbstractViews/Edit", castedModel);
        }

        [HttpPost]
        [Route("{userId}/send-reset-password-mail")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SendResetUserPasswordMail(Guid userId)
        {
            var user = await this.identityService.GetUserByIdAsync<User>(userId);
            if (user != null)
            {
                var resetToken = await this.identityService.GeneratePasswordResetTokenAsync(user);
                var resetPasswordUrl = this.Url.Action("ResetPassword", "AdminAccount", new { userId = userId, resetToken = resetToken }, this.HttpContext.Request.Scheme);

                var mailModel = new UserResetPasswordEmailModel
                {
                    Email = user.Email,
                    Subject = "Password Reset",
                    GivenName = user.FirstName,
                    Surname = user.LastName,
                    resetPasswordUrl = resetPasswordUrl,
                };
                var result = await this.emailService.SendEmailAsync("UserResetPasswordEmailView", mailModel);
                if (result.Success)
                {
                    this.TempData["SuccessStatusMessage"] =
                        $"Password reset email to {mailModel.Email} has been sent successfully.";
                }
                else
                {
                    this.TempData["ErrorStatusMessage"] = $"An error occured while sending email to {mailModel.Email}.";
                }

                return this.RedirectToAction(nameof(this.GetAll));
            }

            return this.NotFound();
        }

        protected override void TableViewActionsInit()
        {
            base.TableViewActionsInit();
            this.TableRowActions.Insert(1, TableMapper.CreateAction("Reset MFA", MaterialDesignIcons.Qrcode, Color.MediumVioletRed, TableRowActionMethod.Post, $"/{this.ControllerRoute}{{0}}/reset-mfa", "[Id]"));
            this.TableRowActions.Insert(2, TableMapper.EditAction($"/{this.ControllerRoute}{{0}}/edit-user", "[Id]"));
            this.TableRowActions.Insert(3, TableMapper.CreateAction("Send Email", MaterialDesignIcons.Email, Color.ForestGreen, TableRowActionMethod.Get, $"/{this.ControllerRoute}{{0}}/send-email", "[Id]"));

            var resetPasswordAction = TableMapper.CreateAction("Reset Password", MaterialDesignIcons.Account, Color.Coral, TableRowActionMethod.Post, $"/{this.ControllerRoute}{{0}}/send-reset-password-mail", "[Id]");
            resetPasswordAction.SetConfirmation("Reset Password", "Are you sure you want to reset password of this user?");
            this.TableRowActions.Insert(4, resetPasswordAction);

            var resetRefreshTokenAction = TableMapper.CreateAction("Reset Refresh Token", MaterialDesignIcons.Refresh, Color.PaleVioletRed, TableRowActionMethod.Post, $"/{this.ControllerRoute}{{0}}/reset-refresh-token", "[Id]");
            resetRefreshTokenAction.SetConfirmation("Reset Refresh Token", "Are you sure you want to reset refresh token of this user?");
            this.TableRowActions.Insert(5, resetRefreshTokenAction);
        }

        protected override void InitNavigationActionsIntoListPage()
        {
            this.NavigationActions.Clear();
            this.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = "Create New User",
                Method = HttpMethod.Get,
                ActionUrl = $"/{this.ControllerRoute}create-new",
                Icon = MaterialDesignIcons.Plus,
            });
        }
    }
}
