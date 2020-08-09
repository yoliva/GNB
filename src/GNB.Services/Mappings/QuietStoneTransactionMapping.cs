using GNB.Core;
using GNB.Services.Dtos;
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
