using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core.UnitOfWork;
using GNB.Infrastructure.Capabilities;
using GNB.QuietStone;
using GNB.Services.Dtos;
using Mapster;
using Microsoft.Extensions.Logging;

namespace GNB.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuietStoneApi _quietStoneApi;
        private readonly ICurrencyNormalizer _normalizer;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IUnitOfWork unitOfWork, IQuietStoneApi quietStoneApi, ICurrencyNormalizer normalizer, ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _quietStoneApi = quietStoneApi;
            _normalizer = normalizer;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactions()
        {
            try
            {
                _logger.LogInformation("Attempt to retrieve transactions from QuietStone");
                var liveData = await _quietStoneApi.GetTransactions();
                _logger.LogInformation("Transactions successfully retrieved from QuietStone");

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

        public async Task<IEnumerable<TransactionDto>> GetTransactionsBySku(string sku, string displayCurrency)
        {
            _logger.LogInformation("Attempt to retrieve transactions from QuietStone by sku", new { sku });

            if (string.IsNullOrEmpty(sku))
                throw new GNBException("Invalid sku provided", ErrorCode.InvalidSku);

            if (string.IsNullOrEmpty(displayCurrency))
                throw new GNBException("Invalid currency provided", ErrorCode.InvalidCurrency);

            var transactions = (await GetTransactions())
                .Where(x => x.Sku == sku);

            _logger.LogInformation("Successfully fetched transactions filtered by sku", new
            {
                sku,
                Count = transactions.Count()
            });
                
            return await _normalizer.Normalize(displayCurrency, transactions);
        }
    }
}
