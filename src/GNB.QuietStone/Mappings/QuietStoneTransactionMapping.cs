using GNB.Core;
using GNB.QuietStone.Dtos;
using Mapster;

namespace GNB.QuietStone.Mappings
{
    public static class QuietStoneTransactionMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<QuietStoneTransactionDto, Transaction>()
                .Map(x => x.Currency, x => x.Currency.ToUpper());
        }
    }
}
