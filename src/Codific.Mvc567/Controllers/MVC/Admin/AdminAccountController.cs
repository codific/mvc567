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
using Codific.Mvc567.CommonCore;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.AdminViewModels;
using Codific.Mvc567.Entities.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly Services.Abstractions.IAuthenticationService authenticationService;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdminAccountController(
            UserManager<User> userManager,
            Services.Abstractions.IAuthenticationService authenticationService,
            IWebHostEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.authenticationService = authenticationService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Route("/admin/login")]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }

            AdminLoginViewModel model = new AdminLoginViewModel();

            return View(model);
        }

        [Route("/admin/login")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(VisibleReCaptchaValidateAttribute))]
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (await this.authenticationService.UserHasAdministrationAccessRightsAsync(user))
                {
                    var result = await this.authenticationService.SignInAsync(user, model.Password, this.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, AuthenticationProperties);

                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction(nameof(LoginWith2fa));
                    }
                    if (result.IsLockedOut)
                    {
                        return RedirectToAction(nameof(Lockout));
                    }
                    if (result.Succeeded)
                    {
                        return AdminDashboardActionResult;
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        [Route("/admin/login-2fa")]
        [HttpGet]
        public async Task<IActionResult> LoginWith2fa()
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }
            var user = await this.authenticationService.GetTwoFactorAuthenticationUserAsync<User>(HttpContext);
            if (user == null || !(await this.authenticationService.UserHasAdministrationAccessRightsAsync(user)))
            {
                return NotFound();
            }

            AdminLoginWith2faViewModel model = new AdminLoginWith2faViewModel();

            return View(model);
        }

        [HttpPost]
        [Route("/admin/login-2fa")]
        [ServiceFilter(typeof(VisibleReCaptchaValidateAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(AdminLoginWith2faViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.authenticationService.GetTwoFactorAuthenticationUserAsync<User>(HttpContext);
            if (user == null || !(await this.authenticationService.UserHasAdministrationAccessRightsAsync(user)))
            {
                return NotFound();
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this.authenticationService.SignInWith2faAsync(user, authenticatorCode, false, HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, AuthenticationProperties);

            if (result.Succeeded)
            {
                return AdminDashboardActionResult;
            }
            else if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View(model);
            }
        }

        [Route("/admin/lockout")]
        [HttpGet]
        public IActionResult Lockout()
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }

            return View();
        }

        [HttpPost]
        [Route("admin/logout")]
        [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.authenticationService.SignOutAsync(HttpContext);

            return RedirectToAction("Index", "Home");
        }

        private IActionResult AdminDashboardActionResult
        {
            get
            {
                return RedirectToAction("Index", "AdminDashboard");
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
                    ExpiresUtc = expiresUtc
                };
            }
        }
    }
}
