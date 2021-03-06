﻿// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
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
using System.Threading.Tasks;
using Codific.Mvc567.DataAccess.Abstractions.Entities;
using Codific.Mvc567.Dtos.Api;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.Abstractions
{
    public abstract class ApiController<TEntity, TEntityDto, TCreateEditEntityDto> : Controller
        where TEntity : class, IEntityBase, new()
        where TEntityDto : class, new()
        where TCreateEditEntityDto : class, new()
    {
        private readonly IEntityManager entityManager;

        public ApiController(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        public IEntityManager EntityManager
        {
            get
            {
                return this.entityManager;
            }
        }

        protected bool HasGetAll { get; set; } = true;

        protected bool HasFilter { get; set; } = true;

        protected bool HasGet { get; set; } = true;

        protected bool HasCreate { get; set; } = true;

        protected bool HasModify { get; set; } = true;

        protected bool HasDelete { get; set; } = true;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<IEnumerable<TEntityDto>>> GetAll()
        {
            if (!this.HasGetAll)
            {
                return this.NotFound();
            }

            var entities = await this.entityManager.GetAllEntitiesAsync<TEntity, TEntityDto>();

            return this.Ok(entities);
        }

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public virtual async Task<ActionResult<PaginatedEntitiesResult<TEntityDto>>> Filter(FilterQueryRequest filterQueryRequest)
        {
            if (!this.HasFilter)
            {
                return this.NotFound();
            }

            if (filterQueryRequest == null || filterQueryRequest.EmptyQuery)
            {
                return this.RedirectToAction(nameof(this.GetAll));
            }

            var result = await this.entityManager.FilterEntitiesAsync<TEntity, TEntityDto>(filterQueryRequest);

            return this.Ok(result);
        }

        [HttpGet]
        [Route("{entityId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<TEntityDto>> Get(Guid entityId)
        {
            if (!this.HasGet)
            {
                return this.NotFound();
            }

            var entity = await this.entityManager.GetEntityAsync<TEntity, TEntityDto>(entityId);
            if (entity != null)
            {
                return this.Ok(entity);
            }

            return this.NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Create([FromBody]TCreateEditEntityDto entity)
        {
            if (!this.HasCreate)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var createdEntityId = await this.entityManager.CreateEntityAsync<TEntity, TCreateEditEntityDto>(entity);
                if (createdEntityId.HasValue)
                {
                    return this.CreatedAtAction(nameof(this.Get), new { entityId = createdEntityId }, new { success = true, entityId = createdEntityId });
                }
            }

            return this.BadRequest(this.ModelState.Select(x => x.Value.Errors).ToArray());
        }

        [HttpPut]
        [Route("{entityId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Modify(Guid entityId, [FromBody]TCreateEditEntityDto entity)
        {
            if (!this.HasModify)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var modifiedEntityId = await this.entityManager.ModifyEntityAsync<TEntity, TCreateEditEntityDto>(entityId, entity);
                if (modifiedEntityId.HasValue)
                {
                    return this.NoContent();
                }
            }

            return this.BadRequest(this.ModelState.Select(x => x.Value.Errors).ToArray());
        }

        [HttpDelete]
        [Route("{entityId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Delete(Guid entityId)
        {
            if (!this.HasDelete)
            {
                return this.NotFound();
            }

            bool isEntityDeleted = await this.entityManager.DeleteEntityAsync<TEntity>(entityId);
            if (isEntityDeleted)
            {
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}
