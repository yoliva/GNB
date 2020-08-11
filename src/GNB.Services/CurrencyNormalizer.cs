using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Infrastructure.Capabilities;
using GNB.Services.Dtos;
using Microsoft.Extensions.Logging;
using static GNB.Services.Utils;

namespace GNB.Services
{
    public class CurrencyNormalizer : ICurrencyNormalizer
    {
        private readonly IRateService _rateService;
        private readonly IRateResolver _rateResolver;
        private readonly ILogger<CurrencyNormalizer> _logger;

        public CurrencyNormalizer(IRateService rateService, IRateResolver rateResolver, ILogger<CurrencyNormalizer> logger)
        {
            _rateService = rateService;
            _rateResolver = rateResolver;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> Normalize(string currency, IEnumerable<TransactionDto> transactions)
        {
            if (string.IsNullOrEmpty(currency))
                throw new GNBException("Currency can not be empty", ErrorCode.InvalidCurrency);

            _logger.LogInformation("Normalizing transactions", new
            {
                currency,
                Count = transactions.Count(),
            });

            var rates = (await _rateService.GetRates()).ToList();
            var ratesDict = rates.ToDictionary(x => (x.From, x.To));

            var missingRates = transactions
                .Where(t => t.Currency != currency && !ratesDict.ContainsKey((t.Currency, currency)))
                .Select(t => (from: t.Currency, to: currency))
                .ToList();

            var ratesDefinition = _rateResolver.GetRatesDefinition(missingRates, rates);
            var normalizedTransactions = transactions.Select(t => new TransactionDto
            {
                ID = t.ID,
                Currency = currency,
                Sku = t.Sku,
                Amount = BankRound(t.Currency != currency ? t.Amount * ratesDefinition[(t.Currency, currency)] : t.Amount)
            });

            _logger.LogInformation("Transactions normalized successfully");

            return normalizedTransactions;
        }
    }
}
