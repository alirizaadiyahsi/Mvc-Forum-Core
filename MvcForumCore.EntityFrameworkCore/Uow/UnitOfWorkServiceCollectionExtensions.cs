using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MvcForumCore.Uow
{
    public static class UnitOfWorkServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddTransient<IRepositoryFactory, Uow.UnitOfWork>();
            services.AddTransient<IUnitOfWork, Uow.UnitOfWork>();

            return services;
        }
    }
}