using GNB.Services.QuietStone;
using Microsoft.Extensions.DependencyInjection;
using System;
using GNB.QuietStone.Mappings;

namespace GNB.QuietStone
{
    public static class Module
    {
        public static IServiceCollection AddQuietStone(this IServiceCollection services,
            Action<QuietStoneConfig> config)
        {
            MapsterConfig.Configure();

            return services
                .Configure(config)
                .AddScoped<IQuietStoneApi, QuietStoneApi>();
        }
    }
}
