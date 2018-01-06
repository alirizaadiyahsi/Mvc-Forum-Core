using System;

namespace MvcForumCore
{
    public abstract class AuditedEntityFull : AuditedEntityCreation
    {
        public Guid UpdateUserId { get; set; }
        public DateTime ModificationDateTime { get; set; }
    }

    public abstract class AuditedEntityCreation : BaseEntity
    {
        public Guid CreateUserId { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}