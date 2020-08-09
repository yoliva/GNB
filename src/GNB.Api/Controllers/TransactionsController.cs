using System.Threading.Tasks;
using GNB.Core;
using GNB.Services;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _transactionService.GetTransactions());
        }

        [HttpGet]
        [Route("sku/{sku}")]
        public async Task<ActionResult> GetBySku(string sku, string currency = KnownCurrencies.Euro)
        {
            return Ok(await _transactionService.GetTransactionsBySku(sku, currency));
        }
    }
}
