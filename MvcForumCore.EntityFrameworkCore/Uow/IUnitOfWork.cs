using System;
using System.Linq;
using System.Threading.Tasks;
using MvcForumCore.Repositories;

namespace MvcForumCore.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        void ChangeDatabase(string database);

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        int SaveChanges(bool ensureAutoHistory = false);

        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        int ExecuteSqlCommand(string sql, params object[] parameters);

        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}
