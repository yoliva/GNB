using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using Mapster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GNB.QuietStone
{
    public class QuietStone : ITransactionDataProvider
    {
        private readonly IQuietStoneApi _quietStoneApi;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<QuietStone> _logger;

        public QuietStone(IQuietStoneApi quietStoneApi, IUnitOfWork unitOfWork, ILogger<QuietStone> logger)
        {
            _quietStoneApi = quietStoneApi;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public string ID => "QuietStone";

        public async Task<List<Rate>> GetRates()
        {
            _logger.LogInformation("Attempt to retrieve current rates from QuietStone");
            var rates = await _quietStoneApi.GetRates();
            _logger.LogInformation("Rates were pulled from QuietStone");

            await _unitOfWork.TransactionTraceRepository.AddAsync(new TransactionTrace
            {
                Status = TraceStatus.Pending,
                TransactionList = JsonConvert.SerializeObject(rates)
            });

            _logger.LogInformation("Rate trace persisted");

            return rates.Select(r => r.Adapt<Rate>()).ToList();
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            _logger.LogInformation("Attempt to retrieve transactions from QuietStone");
            var transactions = await _quietStoneApi.GetTransactions();
            _logger.LogInformation("Transactions were pulled from QuietStone");

            await _unitOfWork.TransactionTraceRepository.AddAsync(new TransactionTrace
            {
                Status = TraceStatus.Pending,
                TransactionList = JsonConvert.SerializeObject(transactions)
            });

            _logger.LogInformation("Transactions trace persisted");

            return transactions.Select(r => r.Adapt<Transaction>()).ToList();
        }
    }
}
