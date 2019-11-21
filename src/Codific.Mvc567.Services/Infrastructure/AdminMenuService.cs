using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class AdminMenuService : AbstractService, IAdminMenuService
    {
        public AdminMenuService(IUnitOfWork uow, IMapper mapper)
            : base (uow, mapper)
        {
        }

        public async Task<PaginatedEntitiesResult<TLinkItemModel>> GetAllLinkItemsAsync<TLinkItemModel>(Guid sectionId)
        {
            try
            {
                var linkItems = await this.StandardRepository.QueryAsync<SidebarNavigationLinkItem>(
                    x => x.ParentSectionId == sectionId,
                    x => x.OrderBy(y => y.Order));

                var linkItemDtos = this.Mapper.Map<IEnumerable<TLinkItemModel>>(linkItems);

                PaginatedEntitiesResult<TLinkItemModel> result = new PaginatedEntitiesResult<TLinkItemModel>();
                result.Entities = linkItemDtos?.ToList();
                result.CurrentPage = 1;
                result.Count = result.EntitiesCount;
                result.PageSize = result.EntitiesCount;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaginatedEntitiesResult<TSectionModel>> GetAllMenuSectionsAsync<TSectionModel>(Guid menuId)
        {
            try
            {
                var sections = await this.StandardRepository.QueryAsync<SidebarMenuSectionItem>(
                    x => x.AdminNavigationSchemeId == menuId,
                    x => x.OrderBy(y => y.Order),
                    x => x.Include(y => y.Children));

                var sectionDtos = this.Mapper.Map<IEnumerable<TSectionModel>>(sections);

                PaginatedEntitiesResult<TSectionModel> result = new PaginatedEntitiesResult<TSectionModel>();
                result.Entities = sectionDtos?.ToList();
                result.CurrentPage = 1;
                result.Count = result.EntitiesCount;
                result.PageSize = result.EntitiesCount;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TShemeModel> GetRoleShemeOrDefaultAsync<TShemeModel>(string role)
        {
            try
            {
                AdminNavigationScheme resultScheme = (await this.StandardRepository.QueryAsync<AdminNavigationScheme>(
                    x => x.Role.Name == role,
                    null,
                    x => x.Include(y => y.Menus).ThenInclude(z => ((SidebarMenuSectionItem)z).Children))).FirstOrDefault();

                if (resultScheme == null)
                {
                    resultScheme = (await this.StandardRepository.QueryAsync<AdminNavigationScheme>(
                    x => !x.RoleId.HasValue,
                    null,
                    x => x.Include(y => y.Menus).ThenInclude(z => ((SidebarMenuSectionItem)z).Children))).FirstOrDefault();
                }
                if (resultScheme == null)
                {
                    return default(TShemeModel);
                }

                resultScheme.Menus = resultScheme.Menus.OrderBy(x => x.Order).ToList();
                foreach (var menuSection in resultScheme.Menus)
                {
                    menuSection.Children = menuSection.Children.OrderBy(x => x.Order).ToList();
                }

                return this.Mapper.Map<TShemeModel>(resultScheme);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetRoleShemeOrDefaultAsync));
                return default(TShemeModel);
            }
        }

        public async Task<TSectionModel> GetSectionByLinkItemIdAsync<TSectionModel>(Guid linkItemId)
        {
            try
            {
                var linkItemEntity = await this.StandardRepository.GetAsync<SidebarNavigationLinkItem>(linkItemId, x => x.Include(y => y.ParentSection));
                if (linkItemEntity != null)
                {
                    return this.Mapper.Map<TSectionModel>(linkItemEntity.ParentSection);
                }

                return default(TSectionModel);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetShemeBySectionIdAsync));
                return default(TSectionModel);
            }
        }

        public async Task<TShemeModel> GetShemeBySectionIdAsync<TShemeModel>(Guid sectionId)
        {
            try
            {
                var sectionEntity = await this.StandardRepository.GetAsync<SidebarMenuSectionItem>(sectionId, x => x.Include(y => y.AdminNavigationScheme));
                if (sectionEntity != null)
                {
                    return this.Mapper.Map<TShemeModel>(sectionEntity.AdminNavigationScheme);
                }

                return default(TShemeModel);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(GetShemeBySectionIdAsync));
                return default(TShemeModel);
            }
        }
    }
}
