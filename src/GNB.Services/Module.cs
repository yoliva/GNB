using GNB.Services.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace GNB.Services
{
    public static class Module
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            MapsterConfig.Configure();

            return services
                .AddScoped<IRateService, RateService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IRateResolver, RateResolver>()
                .AddScoped<ICurrencyNormalizer, CurrencyNormalizer>();
        }
    }
}
