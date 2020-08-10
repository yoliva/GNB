using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.QuietStone.Dtos;

namespace GNB.QuietStone
{
    public interface IQuietStoneApi
    {
        Task<List<QuietStoneRateDto>> GetRates();
        Task<List<QuietStoneTransactionDto>> GetTransactions();
    }
}
