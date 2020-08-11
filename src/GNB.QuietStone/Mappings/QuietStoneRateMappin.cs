using GNB.Core;
using GNB.QuietStone.Dtos;
using Mapster;

namespace GNB.QuietStone.Mappings
{
    public static class QuietStoneRateMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<QuietStoneRateDto, Rate>()
                .Map(x => x.ChangeRate, x => x.Rate);
        }
    }
}
