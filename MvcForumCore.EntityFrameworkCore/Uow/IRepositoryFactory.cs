using MvcForumCore.Repositories;

namespace MvcForumCore.Uow
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}