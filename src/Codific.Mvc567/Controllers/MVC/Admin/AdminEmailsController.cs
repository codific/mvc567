using System;
using System.Drawing;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Codific.Mvc567.Dtos.ViewModels.Mapping;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Codific.Mvc567.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/system/emails/")]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminEmailsController : AdminEntityController<Email, EmailViewModel>
    {
        private readonly IEmailService emailService;

        public AdminEmailsController(IEntityManager entityManager, IEmailService emailService)
            : base(entityManager)
        {
            this.emailService = emailService;

            this.HasEdit = false;
            this.HasGenericCreate = false;
        }

        [HttpPost]
        [Route("{id}/resend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmail(Guid id)
        {
            bool sent = await this.emailService.ResendEmailAsync(id);

            if (sent)
            {
                this.TempData["SuccessStatusMessage"] = "Email has been sent successfully.";
            }
            else
            {
                this.TempData["ErrorStatusMessage"] = "Email has not been sent successfully.";
            }

            return this.RedirectToAction(nameof(this.GetAll));
        }

        protected override void InitNavigationActionsIntoListPage()
        {
            this.NavigationActions.Clear();
        }

        protected override void TableViewActionsInit()
        {
            base.TableViewActionsInit();
            this.TableRowActions.Insert(1, TableMapper.CreateAction(
                                            "Resend Email",
                                            MaterialDesignIcons.Send,
                                            Color.ForestGreen,
                                            TableRowActionMethod.Post,
                                            $"/{this.ControllerRoute}{{0}}/resend",
                                            "[Id]"));
        }
    }
}
