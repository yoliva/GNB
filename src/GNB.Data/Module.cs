using GNB.Core.UnitOfWork;
using GNB.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace GNB.Data
{
    public static class Module
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork,EfUnitOfWork>();
        }
    }
}
