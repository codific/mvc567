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

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mvc567.Common.Utilities;
using Mvc567.DataAccess.Abstraction.Entities;
using Mvc567.Services.Infrastructure;
using Mvc567.Common;
using System.Net.Http;
using Mvc567.Entities.DataTransferObjects.ServiceResults;
using Mvc567.Entities.ViewModels.Abstractions.Table;
using Mvc567.Entities.ViewModels.Mapping;
using Mvc567.Entities.ViewModels.Abstractions;
using Mvc567.Common.Attributes;
using Mvc567.Common.Extensions;

namespace Mvc567.Controllers.Abstractions
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class AbstractEntityController<TEntity, TEntityDto> : Controller
        where TEntity : class, IEntityBase, new()
        where TEntityDto : class, new()
    {
        protected readonly IEntityManager entityManager;
        protected readonly string controllerRoute = string.Empty;
        protected readonly string controllerName = string.Empty;

        protected const string BreadcrumbPageTitlePlaceholder = "[PageTitle]";
        protected const string BreadcrumbEntityNamePluralPlaceholder = "[EntityNamePlural]";

        public AbstractEntityController(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.controllerRoute = ((RouteAttribute)this.GetType().GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault()).Template;
            this.controllerName = this.GetType().Name.Replace("Controller", string.Empty);
        }

        protected bool HasGenericCreate { get; set; } = true;

        protected bool HasDetails { get; set; } = true;

        protected bool HasEdit { get; set; } = true;

        protected bool HasDelete { get; set; } = true;

        [HttpGet]
        [Route("all")]
        [Breadcrumb(BreadcrumbPageTitlePlaceholder, false, 0)]
        public virtual async Task<IActionResult> GetAll([FromQuery(Name = "p")]int page = 1, [FromQuery(Name = "q")]string query = null)
        {
            PaginatedEntitiesResult<TEntityDto> entitiesResult = await this.entityManager.GetAllEntitiesPaginatedAsync<TEntity, TEntityDto>(page, query);
            AllEntitiesViewModel model = new AllEntitiesViewModel();
            model.SingleEntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            model.Title = typeof(TEntity).Name.ToLower().EndsWith("s") ? $"{model.SingleEntityName}es" : $"{model.SingleEntityName}s";
            ViewData[BreadcrumbPageTitlePlaceholder] = model.Title;

            List<TableRowActionViewModel> actions = new List<TableRowActionViewModel>();
            TableViewActionsInit(ref actions);

            model.Table = TableMapper.DtoMapper<TEntityDto>(entitiesResult, actions.ToArray());
            model.Table.SetPaginationRedirection("Admin", this.GetType().Name.Replace("Controller", string.Empty), nameof(GetAll));
            ViewData.Add("searchQuery", query);

            InitNavigationActionsIntoListPage(ref model);

            return View("AbstractViews/GetAll", model);
        }

        protected virtual void InitNavigationActionsIntoListPage(ref AllEntitiesViewModel model)
        {
            model.NavigationActions.Add(new NavigationActionViewModel
            {
                Name = $"Create {model.SingleEntityName}",
                ActionUrl = $"/{GetControllerRoute()}create",
                Icon = MaterialDesignIcons.Plus,
                Method = HttpMethod.Get
            });
        }

        protected string GetControllerRoute()
        {
            var routeAttribute = (RouteAttribute)this.GetType().GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault();

            return routeAttribute.Template;
        }

        [HttpGet]
        [Route("create")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        public virtual async Task<IActionResult> Create()
        {
            if (!HasGenericCreate)
            {
                return NotFound();
            }

            ICreateEditEntityDto model = (ICreateEditEntityDto)new TEntityDto();
            model.Area = "Admin";
            model.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            model.Action = "Create";
            model.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            ViewData[BreadcrumbEntityNamePluralPlaceholder] = model.EntityName.ToPluralString();

            return View("AbstractViews/Create", model);
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Create", false, 1)]
        public virtual async Task<IActionResult> Create(TEntityDto model)
        {
            if (!HasGenericCreate)
            {
                return NotFound();
            }

            ICreateEditEntityDto castedModel = (ICreateEditEntityDto)model;
            castedModel.Area = "Admin";
            castedModel.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            castedModel.Action = "Create";
            castedModel.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            ViewData[BreadcrumbEntityNamePluralPlaceholder] = castedModel.EntityName.ToPluralString();
            ModelState.Remove("Area");
            ModelState.Remove("Controller");
            ModelState.Remove("Action");
            ModelState.Remove("EntityName");

            if (ModelState.IsValid)
            {
                var createdEntityId = await this.entityManager.CreateEntityAsync<TEntity, TEntityDto>(model);
                if (createdEntityId.HasValue)
                {
                    return RedirectToAction("Details", this.controllerName, new { id = createdEntityId });
                }
            }

            return View("AbstractViews/Create", castedModel);
        }

        [HttpGet]
        [Route("{id}/edit")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public virtual async Task<IActionResult> Edit(Guid id)
        {
            if (!HasEdit)
            {
                return NotFound();
            }

            ICreateEditEntityDto model = (ICreateEditEntityDto)(await this.entityManager.GetEntityAsync<TEntity, TEntityDto>(id)); 
            model.Area = "Admin";
            model.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            model.Action = "Edit";
            model.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            ViewData[BreadcrumbEntityNamePluralPlaceholder] = model.EntityName.ToPluralString();

            return View("AbstractViews/Edit", model);
        }

        [HttpPost]
        [Route("{id}/edit")]
        [ValidateAntiForgeryToken]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Edit", false, 1)]
        public virtual async Task<IActionResult> Edit(Guid id, TEntityDto model)
        {
            if (!HasGenericCreate)
            {
                return NotFound();
            }

            ICreateEditEntityDto castedModel = (ICreateEditEntityDto)model;
            castedModel.Area = "Admin";
            castedModel.Controller = this.GetType().Name.Replace("Controller", string.Empty);
            castedModel.Action = "Edit";
            castedModel.EntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            ViewData[BreadcrumbEntityNamePluralPlaceholder] = castedModel.EntityName.ToPluralString();
            ModelState.Remove("Area");
            ModelState.Remove("Controller");
            ModelState.Remove("Action");
            ModelState.Remove("EntityName");

            if (ModelState.IsValid)
            {
                var modifiedEntityId = await this.entityManager.ModifyEntityAsync<TEntity, TEntityDto>(id, model);
                if (modifiedEntityId.HasValue)
                {
                    return RedirectToAction("Details", this.controllerName, new { id = modifiedEntityId });
                }
            }

            return View("AbstractViews/Edit", castedModel);
        }

        [HttpGet]
        [Route("{id}")]
        [Breadcrumb(BreadcrumbEntityNamePluralPlaceholder, true, 0, nameof(GetAll))]
        [Breadcrumb("Details", false, 1)]
        public virtual async Task<IActionResult> Details(Guid id)
        {
            if (!HasDetails)
            {
                return NotFound();
            }

            TEntityDto entity = await this.entityManager.GetEntityAsync<TEntity, TEntityDto>(id);

            EntityDetailsViewModel model = new EntityDetailsViewModel();
            model.Details = DetailsMapper.DtoMapper<TEntityDto>(entity);

            model.Title = $"{StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name)} Details";
            string singleEntityName = StringFunctions.SplitWordsByCapitalLetters(typeof(TEntity).Name);
            ViewData[BreadcrumbEntityNamePluralPlaceholder] = singleEntityName.ToPluralString();

            return View("AbstractViews/Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}/delete")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            if (!HasDelete)
            {
                return NotFound();
            }

            bool isEntityDeleted = await this.entityManager.DeleteEntityAsync<TEntity>(id);
            TempData["EntityDeletedStatus"] = isEntityDeleted;

            return RedirectToAction("GetAll");
        }

        protected virtual void TableViewActionsInit(ref List<TableRowActionViewModel> actions)
        {
            if (HasDetails)
            {
                actions.Add(TableMapper.DetailsAction($"/{this.controllerRoute}{{0}}", "[Id]"));
            }
            if (HasEdit)
            {
                actions.Add(TableMapper.EditAction($"/{this.controllerRoute}{{0}}/edit", "[Id]"));
            }
            if (HasDelete)
            {
                actions.Add(TableMapper.DeleteAction($"/{this.controllerRoute}{{0}}/delete", "[Id]"));
            }
        }
    }
}
