using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactions();
        Task<IEnumerable<TransactionDto>> GetTransactionsBySku(string sku, string displayCurrency);
    }
}
