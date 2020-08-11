using System.Linq;
using System.Threading.Tasks;
using GNB.Api.Models;
using GNB.Core;
using GNB.Services;
using GNB.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITransactionService _transactionService;

        public DashboardController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index(string sku, string currency = KnownCurrencies.EUR)
        {
            return View(new TransactionList
            {
                Sku = sku,
                Transactions = string.IsNullOrEmpty(sku)
                    ? Enumerable.Empty<TransactionDto>()
                    : await _transactionService.GetTransactionsBySku(sku, currency.ToUpper())
            });
        }
    }
}
