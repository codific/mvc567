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

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Mvc567.Common.Utilities;
using Mvc567.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Mvc567.Controllers.Abstractions;
using Mvc567.Common.Attributes;
using Mvc567.Entities.ViewModels.AdminViewModels;
using Mvc567.Entities.Database;
using Mvc567.DataAccess.Identity;

namespace Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminAccountController : AbstractController
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IIdentityService identityService;
        private readonly RoleManager<Role> roleManager;
        private readonly ISignInService signInService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ISingletonSecurityService securityService;

        public AdminAccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IIdentityService identityService,
            ISignInService signInService,
            IConfiguration configuration, 
            IEmailService emailService,
            IHostingEnvironment hostingEnvironment,
            ILanguageService languageService,
            ISingletonSecurityService securityService)
            : base(configuration, emailService, languageService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.identityService = identityService;
            this.signInService = signInService;
            this.hostingEnvironment = hostingEnvironment;
            this.securityService = securityService;
        }

        [Route("/admin/login")]
        [HttpGet]
        [ValidateAdminCookie]
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
        [ValidateAdminCookie]
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

                if (await this.signInService.UserHasAdministrationAccessRightsAsync(user))
                {
                    var result = await this.signInService.SignInAsync(user, model.Password, this.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, AuthenticationProperties);

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
        [ValidateAdminCookie]
        public async Task<IActionResult> LoginWith2fa()
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }
            var user = await this.signInService.GetTwoFactorAuthenticationUserAsync(HttpContext);
            if (user == null || !(await this.signInService.UserHasAdministrationAccessRightsAsync(user)))
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
        [ValidateAdminCookie]
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

            var user = await this.signInService.GetTwoFactorAuthenticationUserAsync(HttpContext);
            if (user == null || !(await this.signInService.UserHasAdministrationAccessRightsAsync(user)))
            {
                return NotFound();
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await this.signInService.SignInWith2faAsync(user, authenticatorCode, model.RememberBrowser, HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, AuthenticationProperties);

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
        [ValidateAdminCookie]
        public IActionResult Lockout()
        {
            if (User.Identity.IsAuthenticated)
            {
                return AdminDashboardActionResult;
            }

            return View();
        }

        [Route("/admin/auth/init")]
        [HttpGet]
        public async Task<IActionResult> Auth()
        {
            int delaySeconds = 5 * this.securityService.AdminLoginFailedAttempts;
            await Task.Delay(delaySeconds * 1000);

            return View();
        }

        [HttpPost]
        [Route("/admin/auth/init")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Auth([FromForm(Name = "AuthCode")]string code)
        {
            int delaySeconds = 5 * this.securityService.AdminLoginFailedAttempts;
            await Task.Delay(delaySeconds * 1000);

            if (!string.IsNullOrEmpty(code) && code == Authenticator.GeneratePin(this.configuration["AdminLoginAuthenticator:SecretKey"]))
            {
                string[] cookieValues = CookiesFunctions.GenerateAdminLoginCookieValues(
                    configuration["AdminLoginAuthenticator:CookieFormat"],
                    new string[] { this.HttpContext.Request.Headers["User-Agent"], DateTime.Now.Year.ToString() },
                    configuration["AdminLoginAuthenticator:SecretIndexes"]);

                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Append(cookieValues[0], cookieValues[1], options);

                return RedirectToAction("Login", "AdminAccount", new { Area = "Admin" });
            }

            this.securityService.IncrementAdminLoginFailedAttempts();

            return NotFound();
        }

        [HttpPost]
        [Route("admin/logout")]
        [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
        [ValidateAntiForgeryToken]
        [ValidateAdminCookie]
        public async Task<IActionResult> Logout()
        {
            await this.signInService.SignOutAsync(HttpContext);

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
