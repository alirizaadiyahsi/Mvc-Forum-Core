using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MvcForumCore.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MvcForumCore.Extensions
{
    /// <summary>
    /// Represents a plugin for Microsoft.EntityFrameworkCore to support automatically recording data changes history.
    /// </summary>
    public static class DbContextExtensions
    {
        private static readonly JsonSerializer Jsonerializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        });

        /// <summary>
        /// Ensures the automatic history.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void EnsureEntityHistory(this DbContext context)
        {
            // Must ToArray() here for excluding the AutoHistory model.
            // Currently, only support Modified and Deleted entity.
            var entries = context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted).ToArray();
            foreach (var entry in entries)
            {
                context.Add(entry.EntityHistory());
            }
        }

        internal static EntityHistory EntityHistory(this EntityEntry entry)
        {
            //TODO: add user id to entity history

            var entityHistory = new EntityHistory
            {
                EntityName = entry.Metadata.Relational().TableName
            };

            // Get the mapped properties for the entity type.
            // (include shadow properties, not include navigations & references)
            var properties = entry.Properties;

            var json = new JObject();
            switch (entry.State)
            {
                case EntityState.Added:
                    foreach (var prop in properties)
                    {
                        if (prop.Metadata.IsKey() || prop.Metadata.IsForeignKey())
                        {
                            continue;
                        }
                        json[prop.Metadata.Name] = prop.CurrentValue != null
                            ? JToken.FromObject(prop.CurrentValue, Jsonerializer)
                            : JValue.CreateNull();
                    }

                    entityHistory.EntityId = entry.PrimaryKey();
                    entityHistory.EntityState = EntityState.Added;
                    entityHistory.ChangeHistory = json.ToString();
                    break;
                case EntityState.Modified:
                    var before = new JObject();
                    var after = new JObject();

                    foreach (var prop in properties)
                    {
                        if (prop.IsModified)
                        {
                            before[prop.Metadata.Name] = prop.OriginalValue != null
                            ? JToken.FromObject(prop.OriginalValue, Jsonerializer)
                            : JValue.CreateNull();

                            after[prop.Metadata.Name] = prop.CurrentValue != null
                            ? JToken.FromObject(prop.CurrentValue, Jsonerializer)
                            : JValue.CreateNull();
                        }
                    }

                    json["before"] = before;
                    json["after"] = after;

                    entityHistory.EntityId = entry.PrimaryKey();
                    entityHistory.EntityState = EntityState.Modified;
                    entityHistory.ChangeHistory = json.ToString();
                    break;
                case EntityState.Deleted:
                    foreach (var prop in properties)
                    {
                        json[prop.Metadata.Name] = prop.OriginalValue != null
                            ? JToken.FromObject(prop.OriginalValue, Jsonerializer)
                            : JValue.CreateNull();
                    }
                    entityHistory.EntityId= entry.PrimaryKey();
                    entityHistory.EntityState = EntityState.Deleted;
                    entityHistory.ChangeHistory = json.ToString();
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    throw new NotSupportedException("AutoHistory only support Deleted and Modified entity.");
            }

            return entityHistory;
        }

        private static Guid PrimaryKey(this EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();

            var values = key.Properties.Select(property => entry.Property(property.Name).CurrentValue)
                .Where(value => value != null)
                .ToList();

            return new Guid(string.Join(",", values));
        }
    }
}
