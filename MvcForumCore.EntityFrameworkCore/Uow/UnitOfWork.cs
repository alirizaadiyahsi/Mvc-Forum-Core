using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MvcForumCore.Extensions;
using MvcForumCore.Repositories;

namespace MvcForumCore.Uow
{
    public class UnitOfWork : IRepositoryFactory, IUnitOfWork
    {
        private bool _disposed;

        public UnitOfWork(DbContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DbContext DbContext { get; }

        public void ChangeDatabase(string database)
        {
            var connection = DbContext.Database.GetDbConnection();
            if (connection.State.HasFlag(ConnectionState.Open))
            {
                connection.ChangeDatabase(database);
            }
            else
            {
                var connectionString = Regex.Replace(connection.ConnectionString.Replace(" ", ""), @"(?<=[Dd]atabase=)\w+(?=;)", database, RegexOptions.Singleline);
                connection.ConnectionString = connectionString;
            }

            // Following code only working for mysql.
            var items = DbContext.Model.GetEntityTypes();
            foreach (var item in items)
            {
                if (item.Relational() is RelationalEntityTypeAnnotations extensions)
                {
                    extensions.Schema = database;
                }
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(DbContext);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return DbContext.Set<TEntity>().FromSql(sql, parameters);
        }

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                DbContext.EnsureEntityHistory();
            }

            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                DbContext.EnsureEntityHistory();
            }

            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}