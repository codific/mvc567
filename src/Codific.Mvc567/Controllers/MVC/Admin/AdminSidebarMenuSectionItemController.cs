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
    [Route("admin/system/navigation-menus/sidebar-menu-section-items/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
    public class AdminSidebarMenuSectionItemController : AdminEntityController<SidebarMenuSectionItem, SidebarMenuSectionItemViewModel>
    {
        private readonly IAdminMenuService adminMenuService;

        public AdminSidebarMenuSectionItemController(IEntityManager entityManager, IAdminMenuService adminMenuService)
            : base(entityManager)
        {
            this.adminMenuService = adminMenuService;
            this.SoftDelete = false;
            this.DeleteRedirectUrlFunction = (id) =>
            {
                string menuSchemeId = this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(id).Result?.Id;
                return $"/admin/system/navigation-menus/sidebar-menu-section-items/{menuSchemeId}/sections";
            };
        }

        public async override Task<IActionResult> GetAll(
            [FromQuery(Name = "p")] int page = 1,
            [FromQuery(Name = "q")] string query = null,
            [FromQuery(Name = "sortBy")] string sortBy = null)
        {
            await Task.Run(() => { });
            return this.NotFound();
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

            actions.Add(TableMapper.CreateAction("Link Items", MaterialDesignIcons.ViewList, Color.DarkSlateBlue, TableRowActionMethod.Get, $"/admin/system/navigation-menus/sidebar-menu-link-items/sections/{{0}}/link-items", "[Id]"));
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

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Create", false, 2)]
        public async override Task<IActionResult> CreateGet(SidebarMenuSectionItemViewModel model = null)
        {
            Guid sectionMenuId = Guid.Empty;
            if (this.HttpContext.Request.Query.ContainsKey("SchemeId") && Guid.TryParse(this.HttpContext.Request.Query["SchemeId"], out _))
            {
                sectionMenuId = Guid.Parse(this.HttpContext.Request.Query["SchemeId"]);
            }

            this.ViewData["[SectionMenuId]"] = sectionMenuId;

            model = new SidebarMenuSectionItemViewModel();
            model.AdminNavigationSchemeId = sectionMenuId;

            return await base.CreateGet(model);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Create", false, 2)]
        public async override Task<IActionResult> CreatePost(SidebarMenuSectionItemViewModel model)
        {
            this.ViewData["[SectionMenuId]"] = model.AdminNavigationSchemeId;
            return await base.CreatePost(model);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Details", false, 2)]
        public async override Task<IActionResult> Details(Guid id)
        {
            this.ViewData["[SectionMenuId]"] = (await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(id))?.Id;
            return await base.Details(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Edit", false, 2)]
        public async override Task<IActionResult> Edit(Guid id)
        {
            this.ViewData["[SectionMenuId]"] = (await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(id))?.Id;
            return await base.Edit(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminSidebarMenuSectionItem", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Edit", false, 2)]
        public async override Task<IActionResult> Edit(Guid id, SidebarMenuSectionItemViewModel model)
        {
            this.ViewData["[SectionMenuId]"] = (await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(id))?.Id;
            return await base.Edit(id, model);
        }
    }
}
