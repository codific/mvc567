using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codific.Mvc567.Dtos.ServiceResults;

namespace Codific.Mvc567.Services.Abstractions
{
    public interface IAdminMenuService
    {
        Task<TShemeModel> GetRoleShemeOrDefaultAsync<TShemeModel>(string role);

        Task<PaginatedEntitiesResult<TSectionModel>> GetAllMenuSectionsAsync<TSectionModel>(Guid menuId);

        Task<TShemeModel> GetShemeBySectionIdAsync<TShemeModel>(Guid sectionId);

        Task<TSectionModel> GetSectionByLinkItemIdAsync<TSectionModel>(Guid linkItemId);

        Task<PaginatedEntitiesResult<TLinkItemModel>> GetAllLinkItemsAsync<TLinkItemModel>(Guid sectionId);
    }
}
