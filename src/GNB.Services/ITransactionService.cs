using System.Collections.Generic;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface ITransactionService
    {
        IEnumerable<TransactionDto> GetTransactions();
        IEnumerable<TransactionDto> GetTransactionsBySku(string sku);
    }
}
