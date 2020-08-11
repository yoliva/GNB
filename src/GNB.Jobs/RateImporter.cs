﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core.Traces;
using GNB.Core.UnitOfWork;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GNB.Jobs
{
    public class RatesImporter : IRatesImporter
    {
        private readonly IServiceProvider _serviceProvider;

        public RatesImporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [AutomaticRetry(Attempts = 0)]
        public void Import()
        {
            Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetService<ILogger<TransactionImporter>>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                logger.LogInformation("Execution request");

                var lastTrace = unitOfWork.RateTraceRepository.GetAll()
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                if (lastTrace is null || lastTrace.Status != TraceStatus.Pending)
                {
                    logger.LogInformation("No rate trace pending found. Execution skipped");
                    return;
                }

                await unitOfWork.RateTraceRepository.Truncate();
                await unitOfWork.Commit();
                logger.LogInformation("Old rates truncated");

                var entries = JsonConvert.DeserializeObject<List<RateTrace>>(lastTrace.RateList);
                await unitOfWork.RateTraceRepository.AddRangeAsync(entries);
                lastTrace.Status = TraceStatus.Processed;

                await unitOfWork.Commit();

                logger.LogInformation("Execution completed", new
                {
                    RateCount = entries.Count
                });
            }).Wait();

        }
    }
}
