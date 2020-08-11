using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GNB.Jobs
{
    public static class Module
    {
        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            return services
                .AddScoped<ITransactionImporter, TransactionImporter>()
                .AddScoped<IRatesImporter, RatesImporter>();
        }
    }

    public static class BgJobs
    {
        public static void RegisterJobs()
        {
            RecurringJob.AddOrUpdate<ITransactionImporter>( x =>  x.Import(), "*/1 * * * *");
            RecurringJob.AddOrUpdate<IRatesImporter>(x => x.Import(), "*/1 * * * *");
        }
    }
}
