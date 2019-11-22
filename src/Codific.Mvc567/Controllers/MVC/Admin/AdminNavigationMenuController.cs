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
using Codific.Mvc567.Dtos.ViewModels;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Codific.Mvc567.Dtos.ViewModels.Mapping;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SidebarMenuSectionItemViewModel = Codific.Mvc567.ViewModels.SidebarMenuSectionItemViewModel;
using SidebarNavigationLinkItemViewModel = Codific.Mvc567.ViewModels.SidebarNavigationLinkItemViewModel;

namespace Codific.Mvc567.Controllers.MVC.Admin
{
    [Area("Admin")]
    [Route("admin/system/navigation-menus/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminNavigationMenuController : AbstractEntityController<AdminNavigationScheme, AdminNavigationSchemeViewModel>
    {
        private readonly IAdminMenuService adminMenuService;

        public AdminNavigationMenuController(IEntityManager entityManager, IAdminMenuService adminMenuService)
            : base(entityManager)
        {
            this.adminMenuService = adminMenuService;
            this.SoftDelete = false;
        }

        [HttpGet]
        [Route("{menuId}/sections")]
        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", false, 1)]
        public async Task<IActionResult> GetMenuSections(Guid menuId)
        {
            PaginatedEntitiesResult<SidebarMenuSectionItemViewModel> entitiesResult = await this.adminMenuService.GetAllMenuSectionsAsync<SidebarMenuSectionItemViewModel>(menuId);
            AllEntitiesViewModel model = new AllEntitiesViewModel();
            model.SingleEntityName = "Sidebar Section";
            model.Title = "Menu Sidebar Sections";
            this.ViewData[BreadcrumbPageTitlePlaceholder] = model.Title;

            List<TableRowActionViewModel> actions = new List<TableRowActionViewModel>();

            actions.Add(TableMapper.CreateAction("Link Items", MaterialDesignIcons.ViewList, Color.DarkSlateBlue, TableRowActionMethod.Get, $"/admin/system/navigation-menus/sections/{{0}}/link-items", "[Id]"));
            actions.Add(TableMapper.DetailsAction($"/admin/system/navigation-menus/sidebar-menu-section-items/{{0}}", "[Id]"));
            actions.Add(TableMapper.EditAction($"/admin/system/navigation-menus/sidebar-menu-section-items/{{0}}/edit", "[Id]"));
            actions.Add(TableMapper.DeleteAction($"/admin/system/navigation-menus/sidebar-menu-section-items/{{0}}/delete", "[Id]"));

            model.Table = TableMapper.DtoMapper<SidebarMenuSectionItemViewModel>(entitiesResult, actions.ToArray());
            model.Table.SetPaginationRedirection("Admin", this.GetType().Name.Replace("Controller", string.Empty), nameof(this.GetAll));

            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = $"Create {model.SingleEntityName}",
                ActionUrl = $"/admin/system/navigation-menus/sidebar-menu-section-items/create?SchemeId={menuId}",
                Icon = MaterialDesignIcons.Plus,
                Method = HttpMethod.Get,
            });

            return this.View("AbstractViews/GetAll", model);
        }

        [HttpGet]
        [Route("sections/{sectionId}/link-items")]
        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", false, 2)]
        public async Task<IActionResult> GetSectionItems(Guid sectionId)
        {
            PaginatedEntitiesResult<SidebarNavigationLinkItemViewModel> entitiesResult = await this.adminMenuService.GetAllLinkItemsAsync<SidebarNavigationLinkItemViewModel>(sectionId);
            AllEntitiesViewModel model = new AllEntitiesViewModel();
            model.SingleEntityName = "Sidebar Section Link Items";
            model.Title = "Menu Sidebar Section Link Items";
            this.ViewData[BreadcrumbPageTitlePlaceholder] = model.Title;

            this.ViewData["[SectionMenuId]"] = (await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(sectionId))?.Id;

            List<TableRowActionViewModel> actions = new List<TableRowActionViewModel>();

            actions.Add(TableMapper.DetailsAction($"/admin/system/navigation-menus/sidebar-menu-link-items/{{0}}", "[Id]"));
            actions.Add(TableMapper.EditAction($"/admin/system/navigation-menus/sidebar-menu-link-items/{{0}}/edit", "[Id]"));
            actions.Add(TableMapper.DeleteAction($"/admin/system/navigation-menus/sidebar-menu-link-items/{{0}}/delete", "[Id]"));

            model.Table = TableMapper.DtoMapper<SidebarNavigationLinkItemViewModel>(entitiesResult, actions.ToArray());
            model.Table.SetPaginationRedirection("Admin", this.GetType().Name.Replace("Controller", string.Empty), nameof(this.GetAll));

            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = $"Create {model.SingleEntityName}",
                ActionUrl = $"/admin/system/navigation-menus/sidebar-menu-link-items/create?SectionId={sectionId}",
                Icon = MaterialDesignIcons.Plus,
                Method = HttpMethod.Get,
            });

            return this.View("AbstractViews/GetAll", model);
        }

        protected override void TableViewActionsInit(ref List<TableRowActionViewModel> actions)
        {
            actions.Add(TableMapper.CreateAction("Sections", MaterialDesignIcons.ViewList, Color.DarkSlateBlue, TableRowActionMethod.Get, $"/admin/system/navigation-menus/{{0}}/sections", "[Id]"));

            base.TableViewActionsInit(ref actions);
        }
    }
}
