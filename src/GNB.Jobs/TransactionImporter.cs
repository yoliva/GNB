using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GNB.Jobs
{
    public class TransactionImporter : ITransactionImporter
    {
        private const int BATCH_SIZE = 5000;

        private readonly IServiceProvider _serviceProvider;

        public TransactionImporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Import()
        {
            Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetService<ILogger<TransactionImporter>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                logger.LogInformation("Execution request");

                var lastTrace = unitOfWork.TransactionTraceRepository.GetAll()
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                if (lastTrace is null || lastTrace.Status != TraceStatus.Pending)
                {
                    logger.LogInformation("No transaction trace pending found. Execution skipped");
                    return;
                }

                await unitOfWork.TransactionRepository.Truncate();
                logger.LogInformation("Old transactions truncated");

                var entries = JsonConvert.DeserializeObject<List<Transaction>>(lastTrace.TransactionList);
                for (int i = 0; i <= entries.Count / BATCH_SIZE; i++)
                {
                    await unitOfWork.TransactionRepository.AddRangeAsync(entries.Skip(BATCH_SIZE * i)
                        .Take(BATCH_SIZE));
                    await unitOfWork.Commit();
                }

                lastTrace.Status = TraceStatus.Processed;
                await unitOfWork.Commit();

                logger.LogInformation("Execution completed", new
                {
                    TransactionCount = entries.Count
                });
            }).Wait();
        }
    }
}
