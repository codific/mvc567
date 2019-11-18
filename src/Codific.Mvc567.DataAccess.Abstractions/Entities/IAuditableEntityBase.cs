using System;
namespace Codific.Mvc567.DataAccess.Abstractions.Entities
{
    public interface IAuditableEntityBase : IEntityBase
    {
        DateTime CreatedOn { get; set; }

        string CreatedBy { get; set; }

        DateTime UpdatedOn { get; set; }

        string UpdatedBy { get; set; }
    }
}
