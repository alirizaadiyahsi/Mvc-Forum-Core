using System.Linq;
using Microsoft.EntityFrameworkCore;
using MvcForumCore.Authorization;
using MvcForumCore.Extensions;
using MvcForumCore.Logs;
using Xunit;

namespace MvcForumCore.EntityFrameworkCore.Tests.EntityHistoryTests
{
    public class EntityHistoryTest : TestBase
    {
        private readonly MvcForumCoreContext _inMemoryContext;

        public EntityHistoryTest()
        {
            _inMemoryContext = GetEmptyDbContext();
        }

        [Fact]
        public void Entity_Add_AutoHistory_Test()
        {
            _inMemoryContext.Add(new ApplicationUser
            {
                UserName = "TestUser",
                Email = "test@mail.com"
            });

            _inMemoryContext.Add(new ApplicationUser
            {
                UserName = "TestUser2",
                Email = "test2@mail.com"
            });

            _inMemoryContext.EnsureEntityHistory();

            var count = _inMemoryContext.ChangeTracker.Entries()
                .Count(e => e.State == EntityState.Added && !e.Metadata.Name.Contains(typeof(EntityHistory).Name));

            Assert.Equal(2, count);
        }

        [Fact]
        public void Entity_Update_AutoHistory_Test()
        {
            var testUser = new ApplicationUser
            {
                UserName = "TestUser"
            };

            _inMemoryContext.Add(testUser);
            _inMemoryContext.SaveChanges();

            testUser.UserName = "UpdateTestUser";
            _inMemoryContext.EnsureEntityHistory();

            var count = _inMemoryContext.ChangeTracker.Entries().Count(e => e.State == EntityState.Modified);

            Assert.Equal(1, count);
        }
    }
}
