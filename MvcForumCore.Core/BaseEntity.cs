using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MvcForumCore
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public static explicit operator BaseEntity(EntityEntry v)
        {
            throw new NotImplementedException();
        }
    }
}
