using Microsoft.Extensions.DependencyInjection;

namespace GNB.Services
{
    public static class Module
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRateService, RateService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}
