using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Services.Dtos;
using Microsoft.Extensions.Logging;

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
            var rates = (await _rateService.GetRates()).ToList();
            var ratesDict = rates.ToDictionary(x => (x.From, x.To));

            var missingRates = transactions
                .Where(t => t.Currency != currency && !ratesDict.ContainsKey((t.Currency, currency)))
                .Select(t => (from: t.Currency, to: currency))
                .ToList();

            var fullRates = _rateResolver.GetRates(missingRates, rates);
            return transactions.Select(t => new TransactionDto
            {
                ID = t.ID,
                Currency = currency,
                Sku = t.Sku,
                Amount = t.Currency != currency ? t.Amount * fullRates[(t.Currency, currency)] : t.Amount
            });
        }
    }
}
