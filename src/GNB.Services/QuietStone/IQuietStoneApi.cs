using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Services.Dtos;
using GNB.Services.QuietStone.Dtos;

namespace GNB.Services.QuietStone
{
    public interface IQuietStoneApi
    {
        Task<IEnumerable<QuietStoneRateDto>> GetRates();
        Task<IEnumerable<QuietStoneTransactionDto>> GetTransactions();
    }
}
