using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MvcForumCore.Authorization;

namespace MvcForumCore.EntityFrameworkCore.Tests
{
    public class TestBase
    {
        public MvcForumCoreContext GetEmptyDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MvcForumCoreContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var inMemoryContext = new MvcForumCoreContext(optionsBuilder.Options);

            return inMemoryContext;
        }

        public MvcForumCoreContext GetInitializedDbContext()
        {
            var inMemoryContext = GetEmptyDbContext();

            var testUsers = new List<ApplicationUser>
            {
                new ApplicationUser {UserName = "A"},
                new ApplicationUser {UserName = "B"},
                new ApplicationUser {UserName = "C"},
                new ApplicationUser {UserName = "D"},
                new ApplicationUser {UserName = "E"},
                new ApplicationUser {UserName = "F"}
            };

            inMemoryContext.AddRange(testUsers);
            inMemoryContext.SaveChanges();

            return inMemoryContext;
        }
    }
}
