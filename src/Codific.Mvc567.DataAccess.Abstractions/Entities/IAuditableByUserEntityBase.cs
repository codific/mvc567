using System;
namespace Codific.Mvc567.DataAccess.Abstractions.Entities
{
    public interface IAuditableByUserEntityBase<TUser> : IAuditableEntityBase
    {
        Guid UserId { get; set; }

        TUser User { get; set; }
    }
}
