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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Codific.Mvc567.Common;
using Codific.Mvc567.Common.Attributes;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Common.Utilities;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Dtos.ViewModels.Abstractions;
using Codific.Mvc567.Dtos.ViewModels.Abstractions.Table;
using Codific.Mvc567.Dtos.ViewModels.Mapping;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.Abstractions
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class AbstractEntityController<TEntity, TEntityDto> : Controller
        where TEntity : class, IEntityBase, new()
        where TEntityDto : class, new()
    {
        protected const string BreadcrumbPageTitlePlaceholder = "[PageTitle]";
        protected const string BreadcrumbEntityNamePluralPlaceholder = "[EntityNamePlural]";
        private readonly IEntityManager entityManager;

        private string controllerRoute = string.Empty;
        private string controllerName = string.Empty;

        public AbstractEntityController(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.controllerRoute = ((RouteAttribute)this.GetType().GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault())?.Template;
            this.controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            this.TableRowActions = new List<TableRowActionViewModel>();
            this.NavigationActions = new List<NavigationActionViewModel>();
        }

        public IEntityManager EntityManager
        {
            get
            {
                return this.entityManager;
            }
        }

        public string ControllerRoute
        {
            get => this.controllerRoute;
            private set => this.controllerRoute = value;
        }

        public string ControllerName
        {
            get => this.controllerName;
            private set => this.controllerName = value;
        }

        protected bool HasGenericCreate { get; set; } = true;

        protected bool HasDetails { get; set; } = true;

        protected bool HasEdit { get; set; } = true;

        protected bool HasDelete { get; set; } = true;

        protected bool HasRestore { get; set; } = true;

        protected bool SoftDelete { get; set; } = true;

        protected List<TableRowActionViewModel> TableRowActions { get; }

        protected List<NavigationActionViewModel> NavigationActions { get; }

        protected Func<Guid, string> DeleteRedirectUrlFunction { get; set; }

        [HttpGet]
        [Route("all")]
        [Breadcrumb(BreadcrumbPageTitlePlaceholder, false, 0)]
        public virtual async Task<IActionResult> GetAll([FromQuery(Name = "p")] int page = 1, [FromQuery(Name = "q")] string query = null, [FromQuery(Name = "d")] bool showDeleted = false)
        {
            PaginatedEntitiesResult<TEntityDto> entitiesResult = await this.entityManager.GetAllEntitiesPaginatedAsync<TEntity, TEntityDto>(page, query, showDeleted);
            AllEntitiesViewModel model = new AllEntitiesViewModel();
            model.SingleEntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            model.Title = typeof(TEntity).Name.ToLower().EndsWith("s", StringComparison.Ordinal) ? $"{model.SingleEntityName}es" : $"{model.SingleEntityName}s";
            this.ViewData[BreadcrumbPageTitlePlaceholder] = model.Title;

            if (showDeleted)
            {
                this.HasDelete = false;
            }

            if (!showDeleted)
            {
                this.HasRestore = false;
            }

            this.TableViewActionsInit();

            model.Table = TableMapper.DtoMapper<TEntityDto>(entitiesResult, this.TableRowActions.ToArray());
            model.Table.SetPaginationRedirection("Admin", this.GetType().Name.Replace("Controller", string.Empty), nameof(this.GetAll));
            this.ViewData.Add("searchQuery", query);

            this.InitNavigationActionsIntoListPage();
            model.NavigationActions = this.NavigationActions;

            return this.View("AbstractViews/GetAll", model);
        }

        [HttpGet]
        [Route("create")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        public virtual async Task<IActionResult> CreateGet(TEntityDto model = null)
        {
            if (!this.HasGenericCreate)
            {
                return this.NotFound();
            }

            if (model is null)
            {
                model = new TEntityDto();
            }

            ((ICreateEditEntityDto)model).Area = "Admin";
            ((ICreateEditEntityDto)model).Controller = await Task.FromResult(this.GetType().Name.Replace("Controller", string.Empty));
            ((ICreateEditEntityDto)model).Action = "CreatePost";
            ((ICreateEditEntityDto)model).EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = ((ICreateEditEntityDto)model).EntityName.ToPluralString();

            this.ModelState.Clear();

            return this.View("AbstractViews/Create", model);
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        public virtual async Task<IActionResult> CreatePost(TEntityDto model)
        {
            if (!this.HasGenericCreate)
            {
                return this.NotFound();
            }

            ICreateEditEntityDto castedModel = (ICreateEditEntityDto)model;
            castedModel.Area = "Admin";
            castedModel.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            castedModel.Action = "CreatePost";
            castedModel.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = castedModel.EntityName.ToPluralString();
            this.ModelState.Remove("Area");
            this.ModelState.Remove("Controller");
            this.ModelState.Remove("Action");
            this.ModelState.Remove("EntityName");

            if (this.ModelState.IsValid)
            {
                var createdEntityId = await this.entityManager.CreateEntityAsync<TEntity, TEntityDto>(model);
                if (createdEntityId.HasValue)
                {
                    return this.RedirectToAction("Details", this.ControllerName, new { id = createdEntityId });
                }
            }

            return this.View("AbstractViews/Create", castedModel);
        }

        [HttpGet]
        [Route("{id}/edit")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public virtual async Task<IActionResult> Edit(Guid id)
        {
            if (!this.HasEdit)
            {
                return this.NotFound();
            }

            ICreateEditEntityDto model = (ICreateEditEntityDto)(await this.entityManager.GetEntityAsync<TEntity, TEntityDto>(id));
            model.Area = "Admin";
            model.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            model.Action = "Edit";
            model.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = model.EntityName.ToPluralString();

            return this.View("AbstractViews/Edit", model);
        }

        [HttpPost]
        [Route("{id}/edit")]
        [ValidateAntiForgeryToken]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public virtual async Task<IActionResult> Edit(Guid id, TEntityDto model)
        {
            if (!this.HasGenericCreate)
            {
                return this.NotFound();
            }

            ICreateEditEntityDto castedModel = (ICreateEditEntityDto)model;
            castedModel.Area = "Admin";
            castedModel.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            castedModel.Action = "Edit";
            castedModel.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = castedModel.EntityName.ToPluralString();
            this.ModelState.Remove("Area");
            this.ModelState.Remove("Controller");
            this.ModelState.Remove("Action");
            this.ModelState.Remove("EntityName");

            if (this.ModelState.IsValid)
            {
                var modifiedEntityId = await this.entityManager.ModifyEntityAsync<TEntity, TEntityDto>(id, model);
                if (modifiedEntityId.HasValue)
                {
                    return this.RedirectToAction("Details", this.ControllerName, new { id = modifiedEntityId });
                }
            }

            return this.View("AbstractViews/Edit", castedModel);
        }

        [HttpPost]
        [Route("x-edit")]
        public virtual async Task<IActionResult> XEdit([FromForm(Name = "pk")] Guid id, [FromForm(Name = "name")] string name, [FromForm(Name = "value")] string value)
        {
            var edited = await this.entityManager.ModifyEntityPropertyAsync<TEntity, TEntityDto>(id, name, value);
            if (edited)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Details", false, 1)]
        public virtual async Task<IActionResult> Details(Guid id)
        {
            if (!this.HasDetails)
            {
                return this.NotFound();
            }

            TEntityDto entity = await this.entityManager.GetEntityAsync<TEntity, TEntityDto>(id);

            EntityDetailsViewModel model = new EntityDetailsViewModel();
            model.Details = DetailsMapper.DtoMapper<TEntityDto>(entity);

            model.Title = $"{StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name)} Details";
            string singleEntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            this.ViewData[BreadcrumbEntityNamePluralPlaceholder] = singleEntityName.ToPluralString();

            return this.View("AbstractViews/Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}/delete")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            if (!this.HasDelete)
            {
                return this.NotFound();
            }

            IActionResult deleteRedirect = this.RedirectToAction("GetAll");

            if (this.DeleteRedirectUrlFunction != null)
            {
                string redirectUrl = this.DeleteRedirectUrlFunction.Invoke(id);
                deleteRedirect = this.Redirect(redirectUrl);
            }

            bool isEntityDeleted = await this.entityManager.DeleteEntityAsync<TEntity>(id, this.SoftDelete);
            this.TempData["EntityDeletedStatus"] = isEntityDeleted;

            return deleteRedirect;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}/restore")]
        public virtual async Task<IActionResult> Restore(Guid id)
        {
            if (!this.HasRestore)
            {
                return this.NotFound();
            }

            bool isEntityDeleted = await this.entityManager.RestoreEntityAsync<TEntity>(id);
            this.TempData["EntityRestoredStatus"] = !isEntityDeleted;

            return this.RedirectToAction("GetAll");
        }

        /// <summary>
        /// Initialize the default table view navigation actions. To add new action add new item to NavigationActions collection.
        /// </summary>
        protected virtual void InitNavigationActionsIntoListPage()
        {
            this.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = $"Create {StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name)}",
                ActionUrl = $"/{this.GetControllerRoute()}create",
                Icon = MaterialDesignIcons.Plus,
                Method = HttpMethod.Get,
            });
        }

        protected string GetControllerRoute()
        {
            var routeAttribute = (RouteAttribute)this.GetType().GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault();

            if (routeAttribute != null)
            {
                return routeAttribute.Template;
            }

            return string.Empty;
        }

        /// <summary>
        /// Initialize the default table view actions. To add new action add new item to TableRowActions collection.
        /// </summary>
        protected virtual void TableViewActionsInit()
        {
            if (this.HasDetails)
            {
                this.TableRowActions.Add(TableMapper.DetailsAction($"/{this.ControllerRoute}{{0}}", "[Id]"));
            }

            if (this.HasEdit)
            {
                this.TableRowActions.Add(TableMapper.EditAction($"/{this.ControllerRoute}{{0}}/edit", "[Id]"));
            }

            if (this.HasDelete)
            {
                this.TableRowActions.Add(TableMapper.DeleteAction($"/{this.ControllerRoute}{{0}}/delete", "[Id]"));
            }

            if (this.HasRestore)
            {
                this.TableRowActions.Add(TableMapper.RestoreAction($"/{this.ControllerRoute}{{0}}/restore", "[Id]"));
            }
        }
    }
}