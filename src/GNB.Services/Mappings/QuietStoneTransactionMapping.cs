using GNB.Core;
using GNB.Services.Dtos;
using GNB.Services.QuietStone.Dtos;
using Mapster;

namespace GNB.Services.Mappings
{
    public static class QuietStoneTransactionMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<QuietStoneTransactionDto, Transaction>();
        }
    }
}
