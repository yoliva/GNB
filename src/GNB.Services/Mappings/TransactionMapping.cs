using GNB.Core;
using GNB.Services.Dtos;
using Mapster;

namespace GNB.Services.Mappings
{
    public static class TransactionMapping
    {
        public static void Configure()
        {
            TypeAdapterConfig.GlobalSettings
                .NewConfig<Transaction, TransactionDto>();
        }
    }
}
