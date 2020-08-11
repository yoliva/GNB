using System.Collections.Generic;
using GNB.Services.Dtos;

namespace GNB.Api.Models
{
    public class TransactionList
    {
        public IEnumerable<TransactionDto> Transactions { get; set; }
        public string Sku { get; set; }
    }
}
