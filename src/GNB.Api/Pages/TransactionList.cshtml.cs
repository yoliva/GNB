using System.Collections.Generic;
using System.Linq;
using GNB.Core;
using GNB.Services;
using GNB.Services.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GNB.Api.Pages
{
    public class TransactionListModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public TransactionListModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IEnumerable<TransactionDto> Transactions { get; set; }
        public string Sku { get; set; }

        public async void OnGet(string sku, string currency = KnownCurrencies.EURO)
        {
            Sku = sku;

            Transactions = string.IsNullOrEmpty(Sku)
                ? Enumerable.Empty<TransactionDto>()
                : await _transactionService.GetTransactionsBySku(Sku, currency);
        }
    }
}