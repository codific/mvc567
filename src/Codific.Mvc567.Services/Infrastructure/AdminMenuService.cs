using System;
using AutoMapper;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Services.Abstractions;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class AdminMenuService : AbstractService, IAdminMenuService
    {
        public AdminMenuService(IUnitOfWork uow, IMapper mapper)
            : base (uow, mapper)
        {
        }


    }
}
