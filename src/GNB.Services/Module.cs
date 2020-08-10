using System;
using GNB.Services.QuietStone;
using Microsoft.Extensions.DependencyInjection;

namespace GNB.Services
{
    public static class Module
    {
        public static IServiceCollection AddServices(this IServiceCollection services, Action<QuietStoneConfig> config)
        {
            return services
                .Configure(config)
                .AddScoped<IRateService, RateService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IRateResolver, RateResolver>()
                .AddScoped<ICurrencyNormalizer, CurrencyNormalizer>()
                .AddScoped<IQuietStoneApi, QuietStoneApi>();
        }
    }
}
