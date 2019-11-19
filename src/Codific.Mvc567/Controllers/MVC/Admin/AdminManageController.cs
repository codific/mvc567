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

using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.AdminViewModels;
using Codific.Mvc567.Entities.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/manage/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminManageController : Controller
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly UserManager<User> userManager;
        private readonly UrlEncoder urlEncoder;

        public AdminManageController(UserManager<User> userManager, UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            this.urlEncoder = urlEncoder;
        }

        [HttpGet]
        [Route("two-factor-authentication")]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var model = new AdminTwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await this.userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
            };

            await this.LoadSharedKeyAndQrCodeUriAsync(user, model);

            return this.View(model);
        }

        [HttpPost]
        [Route("two-factor-authentication")]
        public async Task<IActionResult> TwoFactorAuthentication(AdminTwoFactorAuthenticationViewModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                await this.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return this.View(model);
            }

            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await this.userManager.VerifyTwoFactorTokenAsync(
                user, this.userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2FaTokenValid)
            {
                this.ModelState.AddModelError("Code", "Verification code is invalid.");
                await this.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return this.View(model);
            }

            await this.userManager.SetTwoFactorEnabledAsync(user, true);

            var responseModel = new AdminTwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await this.userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
            };

            this.TempData["SuccessStatusMessage"] = "Two Factor Authenticator has been enabled successfully.";

            return this.View(responseModel);
        }

        [HttpPost]
        [Route("reset-authenticator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.userManager.SetTwoFactorEnabledAsync(user, false);
            await this.userManager.ResetAuthenticatorKeyAsync(user);

            return this.RedirectToAction(nameof(this.TwoFactorAuthentication));
        }

        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            AdminChangePasswordViewModel model = new AdminChangePasswordViewModel();

            return this.View(model);
        }

        [HttpPost]
        [Route("change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.GetUserAsync(this.User);
                var changePasswordResult = await this.userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    this.AddErrors(changePasswordResult);
                }
                else
                {
                    this.TempData["SuccessStatusMessage"] = "Password has been changed successfully.";
                }
            }

            return this.View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                this.urlEncoder.Encode("mvc567"),
                this.urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(User user, AdminTwoFactorAuthenticationViewModel model)
        {
            var unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await this.userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = this.FormatKey(unformattedKey);
            model.AuthenticatorUri = this.GenerateQrCodeUri(user.Email, unformattedKey);
        }
    }
}
