using System.Threading.Tasks;
using MvcForumCore.Authorization;
using MvcForumCore.Repositories;
using MvcForumCore.Uow;
using Xunit;

namespace MvcForumCore.EntityFrameworkCore.Tests.RepositoryTests
{
    public class RepositoryGetPagedListTest : TestBase
    {
        private readonly IRepository<ApplicationUser> _applicationUserRepository;

        public RepositoryGetPagedListTest()
        {
            var inMemoryContext = GetInitializedDbContext();
            IUnitOfWork unitOfWork = new UnitOfWork(inMemoryContext);
            _applicationUserRepository = unitOfWork.GetRepository<ApplicationUser>();
        }

        [Fact]
        public void GetPagedList()
        {
            var page = _applicationUserRepository.GetPagedList(predicate: t => t.UserName == "C", pageSize: 1);

            Assert.Equal(1, page.Items.Count);
        }

        [Fact]
        public async Task GetPagedListAsync()
        {
            var page = await _applicationUserRepository.GetPagedListAsync(predicate: t => t.UserName == "C", pageSize: 1);

            Assert.Equal(1, page.Items.Count);
        }

        //TODO: Add many to many or one to many entities with including testing
        //[Fact]
        //public async Task GetPagedListWithInclude()
        //{
        //    var page = await _applicationUserRepository.GetPagedListAsync(predicate: t => t.UserName == "A", pageSize: 1);

        //    Assert.Equal(1, page.Items.Count);
        //}

        //[Fact]
        //public async Task GetPagedListWithoutInclude()
        //{
        //    var page = await _applicationUserRepository.GetPagedListAsync(pageIndex: 0, pageSize: 1);

        //    Assert.Equal(1, page.Items.Count);
        //}
    }
}
