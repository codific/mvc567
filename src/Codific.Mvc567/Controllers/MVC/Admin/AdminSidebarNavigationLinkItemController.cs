using System;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Controllers.Abstractions;
using Codific.Mvc567.DataAccess.Identity;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
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
    public class AdminSidebarNavigationLinkItemController : AbstractEntityController<SidebarNavigationLinkItem, SidebarNavigationLinkItemViewModel>
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
                return $"/admin/system/navigation-menus/sections/{sectionId}/link-items";
            };
        }

        public async override Task<IActionResult> GetAll([FromQuery(Name = "p")] int page = 1, [FromQuery(Name = "q")] string query = null, [FromQuery(Name = "d")] bool showDeleted = false)
        {
            await Task.Run(() => { });
            return this.NotFound();
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminNavigationMenu", "sectionId", "[SectionId]")]
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
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminNavigationMenu", "sectionId", "[SectionId]")]
        [Breadcrumb("Create", false, 3)]
        public async override Task<IActionResult> CreatePost(SidebarNavigationLinkItemViewModel model)
        {
            var sectionScheme = await this.adminMenuService.GetShemeBySectionIdAsync<AdminNavigationSchemeViewModel>(model.ParentSectionId);

            this.ViewData["[SectionMenuId]"] = sectionScheme?.Id;
            this.ViewData["[SectionId]"] = model.ParentSectionId;

            return await base.CreatePost(model);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminNavigationMenu", "sectionId", "[SectionId]")]
        [Breadcrumb("Details", false, 3)]
        public async override Task<IActionResult> Details(Guid id)
        {
            await this.ApplyBreadcrumbsViewDataMetaAsync(id);
            return await base.Details(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminNavigationMenu", "sectionId", "[SectionId]")]
        [Breadcrumb("Edit", false, 3)]
        public async override Task<IActionResult> Edit(Guid id)
        {
            await this.ApplyBreadcrumbsViewDataMetaAsync(id);
            return await base.Edit(id);
        }

        [Breadcrumb("Admin Navigation Schemes", true, 0, "GetAll", "AdminNavigationMenu")]
        [Breadcrumb("Sidebar Menu Sections", true, 1, "GetMenuSections", "AdminNavigationMenu", "menuId", "[SectionMenuId]")]
        [Breadcrumb("Sidebar Section Link Items", true, 2, "GetSectionItems", "AdminNavigationMenu", "sectionId", "[SectionId]")]
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
