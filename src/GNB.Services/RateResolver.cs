using System;
using System.Collections.Generic;
using System.Linq;
using GNB.Infrastructure.Capabilities;
using GNB.Services.Dtos;
using Microsoft.Extensions.Logging;

namespace GNB.Services
{
    public class RateResolver : IRateResolver
    {
        private readonly ILogger<RateResolver> _logger;

        public RateResolver(ILogger<RateResolver> logger)
        {
            _logger = logger;
        }

        public Dictionary<(string, string), decimal> GetRatesDefinition(List<(string from, string to)> missingRates, List<RateDto> availableRates)
        {
            if (missingRates is null)
                throw new ArgumentNullException(nameof(missingRates));
            
            if(availableRates is null)
                throw new ArgumentNullException();

            var rates = availableRates
                .ToDictionary(x => (x.From, x.To), x => x.Rate);

            foreach (var (from, to) in missingRates)
                UpdateRate(from, to, rates);

            return rates;
        }

        private void UpdateRate(string from, string to, Dictionary<(string From, string To), decimal> rates)
        {
            if (string.IsNullOrEmpty(from))
                throw new ArgumentException(nameof(from));
            
            if (string.IsNullOrEmpty(to))
                throw new ArgumentException(nameof(to));

            var visited = new HashSet<string>();

            if (rates.ContainsKey((from, to)))
                return;

            var ratesQueue = new Queue<(string currency, decimal currentRate, decimal cumulativeRate)>();
            EnqueueNeighbors(from, 1, rates, ratesQueue);

            while (ratesQueue.Any())
            {
                var (currency, currentRate, cumulativeRate) = ratesQueue.Dequeue();
                var currentExchange = rates.TryGetValue((currency, to), out var exchangeRate);

                if (currentExchange)
                {
                    rates[(from, to)] = cumulativeRate * exchangeRate;
                    break;
                }

                if (visited.Contains(currency))
                    continue;

                visited.Add(currency);
                EnqueueNeighbors(currency, cumulativeRate, rates, ratesQueue);
            }

            if (!rates.ContainsKey((from, to)))
            {
                const string msg = "Unable to resolve exchange rate";

                _logger.LogError(msg, new {from, to});
                throw new GNBException(msg, ErrorCode.ExchangeRateNotFound);
            }
        }

        private static void EnqueueNeighbors(string currency, decimal cumulativeRate,
            Dictionary<(string From, string To), decimal> rates,
            Queue<(string currency, decimal currentRate, decimal cumulativeRate)> ratesQueue)
        {
            var exchanges = rates.Where(x => x.Key.From == currency).Select(x => (x.Key.To, x.Value));
            foreach (var (nextCurrency, rate) in exchanges)
                ratesQueue.Enqueue((nextCurrency, rate, cumulativeRate * rate));
        }
    }
}
