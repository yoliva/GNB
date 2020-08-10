using System.Collections.Generic;
using System.Linq;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GNB.Jobs
{
    public class RatesImporter : IRatesImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RatesImporter> _logger;

        public RatesImporter(IUnitOfWork unitOfWork, ILogger<RatesImporter> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void Import()
        {
            _logger.LogInformation("Execution request");

            var lastTrace = _unitOfWork.RateTraceRepository.GetAll()
                .OrderByDescending(x => x.LastUpdatedAt)
                .FirstOrDefault();

            if (lastTrace is null || lastTrace.Status == TraceStatus.Pending)
            {
                _logger.LogInformation("No rate pending found. Execution skipped");
                return;
            }

            var entries = JsonConvert.DeserializeObject<List<RateTrace>>(lastTrace.RateList);
            _unitOfWork.RateTraceRepository.AddRangeAsync(entries);
            lastTrace.Status = TraceStatus.Processed;

            _unitOfWork.Commit();

            _logger.LogInformation("Execution completed", new
            {
                RateCount = entries.Count
            });
        }
    }
}
