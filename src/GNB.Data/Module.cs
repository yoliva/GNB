using GNB.Core.UnitOfWork;
using GNB.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GNB.Data
{
    public static class Module
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.TryAddScoped<IUnitOfWork, EfUnitOfWork>();

            return services;
        }
    }
}
