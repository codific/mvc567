using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
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
    [Route("admin/system/navigation-menus/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminNavigationMenuController : AdminEntityController<AdminNavigationScheme, AdminNavigationSchemeViewModel>
    {
        public AdminNavigationMenuController(IEntityManager entityManager)
            : base(entityManager)
        {
            this.SoftDelete = false;
        }

        protected override void TableViewActionsInit()
        {
            this.TableRowActions.Add(TableMapper.CreateAction("Sections", MaterialDesignIcons.ViewList, Color.DarkSlateBlue, TableRowActionMethod.Get, $"/admin/system/navigation-menus/sidebar-menu-section-items/{{0}}/sections", "[Id]"));

            base.TableViewActionsInit();
        }
    }
}
