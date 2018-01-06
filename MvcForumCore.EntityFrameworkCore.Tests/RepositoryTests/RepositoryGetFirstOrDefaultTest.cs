using MvcForumCore.Authorization;
using MvcForumCore.Repositories;
using MvcForumCore.Uow;
using Xunit;

namespace MvcForumCore.EntityFrameworkCore.Tests.RepositoryTests
{
    public class RepositoryGetFirstOrDefaultTest : TestBase
    {
        private readonly IRepository<ApplicationUser> _applicationUserRepository;

        public RepositoryGetFirstOrDefaultTest()
        {
            var inMemoryContext = GetInitializedDbContext();
            IUnitOfWork unitOfWork = new UnitOfWork(inMemoryContext);
            _applicationUserRepository = unitOfWork.GetRepository<ApplicationUser>();
        }

        [Fact]
        public async void TestGetFirstOrDefaultAsyncGetsCorrectItem()
        {
            var user = await _applicationUserRepository.GetFirstOrDefaultAsync(predicate: t => t.UserName == "A");
            Assert.NotNull(user);
            Assert.Equal("A", user.UserName);
        }

        [Fact]
        public async void TestGetFirstOrDefaultAsyncReturnsNullValue()
        {
            var user = await _applicationUserRepository.GetFirstOrDefaultAsync(predicate: t => t.UserName == "Easy-E");
            Assert.Null(user);
        }

        [Fact]
        public async void TestGetFirstOrDefaultAsyncCanInclude()
        {
            var user = await _applicationUserRepository.GetFirstOrDefaultAsync(
                predicate: c => c.UserName == "A");
            Assert.NotNull(user);
        }
    }
}
