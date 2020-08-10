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

        public async void OnGet(string sku, string currency = KnownCurrencies.EURO)
        {
            await _transactionService.GetTransactionsBySku(sku, currency);
            //Transactions = string.IsNullOrEmpty(sku)
            //    ? Enumerable.Empty<TransactionDto>()
            //    : await _transactionService.GetTransactionsBySku(sku, currency);
        }
    }
}