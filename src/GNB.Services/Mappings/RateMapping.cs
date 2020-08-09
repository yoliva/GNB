using GNB.Core;
using GNB.Services.Dtos;
using Mapster;

namespace GNB.Services.Mappings
{
    public static class RateMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<Rate, RateDto>()
                .Map(x => x.Rate, x => x.ChangeRate);
        }
    }
}
