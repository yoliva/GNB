using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core.UnitOfWork;
using GNB.Services.Dtos;
using GNB.Services.QuietStone;
using Mapster;
using Microsoft.Extensions.Logging;

namespace GNB.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuietStoneApi _quietStoneApi;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IUnitOfWork unitOfWork, IQuietStoneApi quietStoneApi, ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _quietStoneApi = quietStoneApi;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactions()
        {
            try
            {
                _logger.LogInformation("Attempt to retrieve transactions from QuietStone");
                var liveData = await _quietStoneApi.GetTransactions();
                return liveData.Select(x => x.Adapt<TransactionDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching transactions from QuietStone. Returning data from DB instead", ex);
                return _unitOfWork.TransactionRepository.GetAll()
                    .Select(x => x.Adapt<TransactionDto>())
                    .AsEnumerable();
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsBySku(string sku)
        {
            _logger.LogInformation("Attempt to retrieve transactions from QuietStone by sky", new {sku});
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentNullException(nameof(sku));

            return (await GetTransactions())
                .Where(x => x.Sku == sku);
        }
    }
}
