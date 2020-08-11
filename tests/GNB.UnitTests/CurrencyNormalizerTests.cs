using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core;
using GNB.Services;
using GNB.Services.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GNB.UnitTests
{
    public class CurrencyNormalizerTests
    {
        private readonly ICurrencyNormalizer _currencyNormalizer;
        private readonly List<TransactionDto> _transactions;

        public CurrencyNormalizerTests()
        {
            var rateService = new Mock<IRateService>();
            rateService.Setup(x => x.GetRates())
                .Returns(Task.Run(() => new List<RateDto>
                {
                    new RateDto{From = KnownCurrencies.USD,To = KnownCurrencies.CAD,}
                }.AsEnumerable()));

            var rateResolverMock = new Mock<IRateResolver>();
            rateResolverMock
                .Setup(x => x.GetRatesDefinition(It.IsAny<List<(string, string)>>(), It.IsAny<List<RateDto>>()))
                .Returns(new Dictionary<(string, string), decimal>()
                {
                    {(KnownCurrencies.EUR, KnownCurrencies.CAD), 0.99m},
                    {(KnownCurrencies.CAD, KnownCurrencies.EUR), 1.01m},
                    {(KnownCurrencies.USD, KnownCurrencies.EUR), 1.47m},
                    {(KnownCurrencies.EUR, KnownCurrencies.USD), 0.68m},
                    {(KnownCurrencies.CAD, KnownCurrencies.USD), 0.76m},
                    {(KnownCurrencies.USD, KnownCurrencies.CAD), 1.32m},
                });

            _currencyNormalizer = new CurrencyNormalizer(rateService.Object, rateResolverMock.Object,
                new Mock<ILogger<CurrencyNormalizer>>().Object);

            _transactions = new List<TransactionDto>
            {
                new TransactionDto {Amount = 25.10m, Currency = KnownCurrencies.CAD, Sku = "AX10"},
                new TransactionDto {Amount = 12.50m, Currency = KnownCurrencies.USD, Sku = "AX11"},
                new TransactionDto {Amount = 6.99m, Currency = KnownCurrencies.EUR, Sku = "AX12"}
            };
        }

        [Fact]
        public async void Transactions_Are_Normalized_To_USD()
        {
            var transactions = await _currencyNormalizer.Normalize(KnownCurrencies.USD, _transactions);

            var ax10 = transactions.Single(x => x.Sku == "AX10");
            var ax11 = transactions.Single(x => x.Sku == "AX11");
            var ax12 = transactions.Single(x => x.Sku == "AX12");

            Assert.Equal(3,transactions.Count());

            Assert.Equal(KnownCurrencies.USD,ax10.Currency);
            Assert.Equal(19.08m, ax10.Amount);

            Assert.Equal(KnownCurrencies.USD, ax11.Currency);
            Assert.Equal(12.5m, ax11.Amount);

            Assert.Equal(KnownCurrencies.USD, ax12.Currency);
            Assert.Equal(4.75m, ax12.Amount);
        }
    }
}
