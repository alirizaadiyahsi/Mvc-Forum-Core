using System;
using Microsoft.EntityFrameworkCore;

namespace MvcForumCore.Logs
{
    public class EntityHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the source entity id
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the source entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the json about the changing
        /// </summary>
        public string ChangeHistory { get; set; }

        /// <summary>
        /// Gets or sets the entity change state
        /// </summary>
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Gets or sets entity change creation datetime
        /// </summary>
        public DateTime CreationDateTime { get; set; }
    }
}
