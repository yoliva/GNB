using System.Collections.Generic;
using System.Threading.Tasks;

namespace GNB.Core
{
    public interface ITransactionDataProvider
    {
        public string ID { get; }
        Task<List<Rate>> GetRates();
        Task<List<Transaction>> GetTransactions();
    }
}
