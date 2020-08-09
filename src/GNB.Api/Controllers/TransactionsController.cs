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
        public ActionResult Get()
        {
            return Ok(_transactionService.GetTransactions());
        }

        [HttpGet]
        [Route("sku/{sku}")]
        public ActionResult GetBySku(string sku)
        {
            return Ok(_transactionService.GetTransactionsBySku(sku));
        }
    }
}
