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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.EmailModels;
using Codific.Mvc567.Dtos.ViewModels.AdminViewModels;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using VisibleReCaptchaValidateAttribute = Codific.Mvc567.CommonCore.VisibleReCaptchaValidateAttribute;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly Services.Abstractions.IAuthenticationService authenticationService;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IIdentityService identityService;
        private readonly IEmailService emailService;

        public AdminAccountController(
            UserManager<User> userManager,
            Services.Abstractions.IAuthenticationService authenticationService,
            IWebHostEnvironment hostingEnvironment,
            IIdentityService identityService,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
            this.hostingEnvironment = hostingEnvironment;
            this.identityService = identityService;
            this.emailService = emailService;
        }

        private IActionResult AdminDashboardActionResult
        {
            get
            {
                return this.RedirectToAction("Index", "AdminDashboard");
            }
        }

        private AuthenticationProperties AuthenticationProperties
        {
            get
            {
                DateTime expiresUtc = DateTime.UtcNow.AddMinutes(90);
                if (this.hostingEnvironment.IsDevelopment())
                {
                    expiresUtc = DateTime.UtcNow.AddDays(7);
                }

                return new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = expiresUtc,
                };
            }
        }

        [Route("/admin/login")]
        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            AdminLoginViewModel model = new AdminLoginViewModel();

            return this.View(model);
        }

        [Route("/admin/login")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (await this.authenticationService.UserHasAdministrationAccessRightsAsync(user))
                {
                    var result = await this.authenticationService.SignInAsync(user, model.Password, this.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, this.AuthenticationProperties);

                    if (result.RequiresTwoFactor)
                    {
                        return this.RedirectToAction(nameof(this.LoginWith2fa));
                    }

                    if (result.IsLockedOut)
                    {
                        return this.RedirectToAction(nameof(this.Lockout));
                    }

                    if (result.Succeeded)
                    {
                        return this.AdminDashboardActionResult;
                    }
                }

                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(model);
            }

            return this.View(model);
        }

        [Route("/admin/login-2fa")]
        [HttpGet]
        public async Task<IActionResult> LoginWith2fa()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            var user = await this.authenticationService.GetTwoFactorAuthenticationUserAsync<User>(this.HttpContext);
            if (user == null || !(await this.authenticationService.UserHasAdministrationAccessRightsAsync(user)))
            {
                return this.NotFound();
            }

            AdminLoginWith2faViewModel model = new AdminLoginWith2faViewModel();

            return this.View(model);
        }

        [HttpPost]
        [Route("/admin/login-2fa")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(AdminLoginWith2faViewModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.authenticationService.GetTwoFactorAuthenticationUserAsync<User>(this.HttpContext);
            if (user == null || !(await this.authenticationService.UserHasAdministrationAccessRightsAsync(user)))
            {
                return this.NotFound();
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this.authenticationService.SignInWith2faAsync(user, authenticatorCode, false, this.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, this.AuthenticationProperties);

            if (result.Succeeded)
            {
                return this.AdminDashboardActionResult;
            }
            else if (result.IsLockedOut)
            {
                return this.RedirectToAction(nameof(this.Lockout));
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return this.View(model);
            }
        }

        [Route("/admin/lockout")]
        [HttpGet]
        public IActionResult Lockout()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            return this.View();
        }

        [HttpPost]
        [Route("admin/logout")]
        [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.authenticationService.SignOutAsync(this.HttpContext);

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/admin/reset-password")]
        public IActionResult ResetPassword([FromQuery(Name = "userId")]Guid userId, [FromQuery(Name = "resetToken")]string resetToken)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            if (userId == Guid.Empty || string.IsNullOrEmpty(resetToken))
            {
                return this.NotFound();
            }

            return this.View(new AdminResetPasswordViewModel
            {
                PasswordResetToken = resetToken,
                UserId = userId,
            });
        }

        [HttpPost]
        [Route("/admin/reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AdminResetPasswordViewModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.AdminDashboardActionResult;
            }

            if (this.ModelState.IsValid)
            {
                var result = await this.identityService.ResetPasswordAsync(model.UserId, model.PasswordResetToken, model.NewPassword);
                if (result)
                {
                    this.TempData["SuccessStatusMessage"] = "You successfully changed your password!";
                    return this.Redirect(nameof(this.Login));
                }
            }

            return this.View(model);
        }
    }
}