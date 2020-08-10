using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface ICurrencyNormalizer
    {
        Task<IEnumerable<TransactionDto>> Normalize(string currency, IEnumerable<TransactionDto> transactions);
    }
}
