using System.Linq;
using System.Threading.Tasks;
using MvcForumCore.PagedList.Extensions;
using Xunit;

namespace MvcForumCore.EntityFrameworkCore.Tests.PagedListTests
{
    public class PageListExtensionsTests : TestBase
    {
        private readonly MvcForumCoreContext _inMemoryContext;

        public PageListExtensionsTests()
        {
            _inMemoryContext = GetInitializedDbContext();
        }

        [Fact]
        public async Task ToPagedListAsyncTest()
        {
            var page = await _inMemoryContext.Users.Where(u => u.UserName != null).ToPagedListAsync(1, 2);

            Assert.NotNull(page);
            Assert.Equal(6, page.TotalCount);
            Assert.Equal(2, page.Items.Count);
            Assert.Equal("C", page.Items[0].UserName);

            page = await _inMemoryContext.Users.Where(u => u.UserName != null).ToPagedListAsync(0, 2);

            Assert.NotNull(page);
            Assert.Equal(6, page.TotalCount);
            Assert.Equal(2, page.Items.Count);
            Assert.Equal("A", page.Items[0].UserName);
        }
    }
}
