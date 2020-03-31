using System;
using System.Collections.Generic;
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
    [Route("admin/system/navigation-menus/sidebar-menu-link-items/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminSidebarNavigationLinkItemController : AdminEntityController<SidebarNavigationLinkItem, SidebarNavigationLinkItemViewModel>
    {
        private readonly IAdminMenuService adminMenuService;

        public AdminSidebarNavigationLinkItemController(IEntityManager entityManager, IAdminMenuService adminMenuService)
            : base(entityManager)
        {
            this.adminMenuService = adminMenuService;
            this.SoftDelete = false;
            this.DeleteRedirectUrlFunction = (id) =>
            {
                string sectionId = this.adminMenuService.GetSectionByLinkItemIdAsync<SidebarMenuSectionItemViewModel>(id).Result?.Id;
                return $"/admin/system/navigation-menus/sidebar-menu-link-items/sections/{sectionId}/link-items";
            };
        }

        public async override Task<IActionResult> GetAll([FromQuery(Name = "p")] int page = 1, [FromQuery(Name = "q")] string query = null)
        {
            await Task.Run(() => { });
            return this.NotFound();
        }

        [HttpGet]
        [Route("sections/{sectionId}/link-items")]
        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
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

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminSidebarNavigationLinkItem", "sectionId", "[SectionId]")]
        [Breadcrumb("Create", false, 3)]
        public async override Task<IActionResult> CreateGet(SidebarNavigationLinkItemViewModel model = null)
        {
            Guid sectionId = Guid.Empty;
            if (this.HttpContext.Request.Query.ContainsKey("SectionId") && Guid.TryParse(this.HttpContext.Request.Query["SectionId"], out _))
            {
                sectionId = Guid.Parse(this.HttpContext.Request.Query["SectionId"]);
            }

            var sectionScheme = await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(sectionId);

            this.ViewData["[SectionMenuId]"] = sectionScheme?.Id;
            this.ViewData["[SectionId]"] = sectionId;

            model = new SidebarNavigationLinkItemViewModel();
            model.ParentSectionId = sectionId;

            return await base.CreateGet(model);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminSidebarNavigationLinkItem", "sectionId", "[SectionId]")]
        [Breadcrumb("Create", false, 3)]
        public async override Task<IActionResult> CreatePost(SidebarNavigationLinkItemViewModel model)
        {
            var sectionScheme = await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(model.ParentSectionId);

            this.ViewData["[SectionMenuId]"] = sectionScheme?.Id;
            this.ViewData["[SectionId]"] = model.ParentSectionId;

            return await base.CreatePost(model);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminSidebarNavigationLinkItem", "sectionId", "[SectionId]")]
        [Breadcrumb("Details", false, 3)]
        public async override Task<IActionResult> Details(Guid id)
        {
            await this.ApplyBreadcrumbsViewDataMetaAsync(id);
            return await base.Details(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminSidebarNavigationLinkItem", "sectionId", "[SectionId]")]
        [Breadcrumb("Edit", false, 3)]
        public async override Task<IActionResult> Edit(Guid id)
        {
            await this.ApplyBreadcrumbsViewDataMetaAsync(id);
            return await base.Edit(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminSidebarNavigationLinkItem", "sectionId", "[SectionId]")]
        [Breadcrumb("Edit", false, 3)]
        public async override Task<IActionResult> Edit(Guid id, SidebarNavigationLinkItemViewModel model)
        {
            await this.ApplyBreadcrumbsViewDataMetaAsync(id);
            return await base.Edit(id, model);
        }

        private async Task ApplyBreadcrumbsViewDataMetaAsync(Guid linkItemId)
        {
            Guid sectionId = Guid.Parse((await this.adminMenuService.GetSectionByLinkItemIdAsync<SidebarMenuSectionItemViewModel>(linkItemId))?.Id);
            var sectionScheme = await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(sectionId);
            this.ViewData["[SectionMenuId]"] = sectionScheme?.Id;
            this.ViewData["[SectionId]"] = sectionId;
        }
    }
}
