using System.Collections.Generic;
using System.Linq;
using GNB.Core;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GNB.Jobs
{
    public class TransactionImporter : ITransactionImporter
    {
        private const int BATCH_SIZE = 5000;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionImporter> _logger;

        public TransactionImporter(IUnitOfWork unitOfWork, ILogger<TransactionImporter> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Import()
        {
            _logger.LogInformation("Execution request");

            var lastTrace = _unitOfWork.TransactionTraceRepository.GetAll()
                .OrderByDescending(x => x.LastUpdatedAt)
                .FirstOrDefault();

            if (lastTrace is null)
            {
                _logger.LogInformation("No trace pending found. Execution skipped");
                return;
            }

            var entries = JsonConvert.DeserializeObject<List<Transaction>>(lastTrace.TransactionList);
            for (int i = 0; i <= entries.Count/BATCH_SIZE; i++)
            {
                _unitOfWork.TransactionRepository.AddRangeAsync(entries.Skip(BATCH_SIZE * i).Take(BATCH_SIZE));
                _unitOfWork.Commit();
            }

            lastTrace.Status = TraceStatus.Processed;
            _unitOfWork.Commit();

            _logger.LogInformation("Execution completed", new
            {
                TransactionCount = entries.Count
            });
        }
    }
}
