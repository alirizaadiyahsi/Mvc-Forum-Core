using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcForumCore.Authorization;
using MvcForumCore.Logs;

namespace MvcForumCore
{
    public class MvcForumCoreContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public MvcForumCoreContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<EntityHistory> EntityHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntityHistory>().ToTable("EntityHistory");

            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserToken");
        }
    }
}