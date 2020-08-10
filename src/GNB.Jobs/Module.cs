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
                .AddScoped<ITransactionImporter, TransactionImporter>();
        }

        public static IApplicationBuilder RegisterJobs(this IApplicationBuilder appBuilder)
        {
            RecurringJob.AddOrUpdate<ITransactionImporter>(x => x.Import(), "*/5 * * * *");
            RecurringJob.AddOrUpdate<IRatesImporter>(x => x.Import(), "*/3 * * * *");

            return appBuilder;
        }
    }
}
