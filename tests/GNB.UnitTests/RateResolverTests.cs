using System;
using System.Collections.Generic;
using GNB.Services;
using GNB.Services.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GNB.UnitTests
{
    public class RateResolverTests
    {
        private readonly RateResolver _rateResolver;
        private readonly List<RateDto> _availableRates;

        public RateResolverTests()
        {
            _rateResolver = new RateResolver(new Mock<ILogger<RateResolver>>().Object);

            _availableRates = new List<RateDto>
            {
                new RateDto {From = "EUR", To = "USD", Rate = 1.359m},
                new RateDto {From = "CAD", To = "EUR", Rate = 0.732m},
                new RateDto {From = "USD", To = "EUR", Rate = 0.736m},
                new RateDto {From = "EUR", To = "CAD", Rate = 1.366m},
                new RateDto {From = "UYU", To = "CAD", Rate = 0.5m},
                new RateDto {From = "CAD", To = "CUC", Rate = 1.2m},
                new RateDto {From = "CUC", To = "EUR", Rate = 15.65m},
                new RateDto {From = "EUR", To = "JPY", Rate = 7.5m},
                new RateDto {From = "GBP", To = "CAD", Rate = 0.5m}
            };
        }

        [Theory]
        [InlineData("EUR", "USD", 1.359)]
        [InlineData("EUR", "CAD", 1.366)]
        [InlineData("UYU", "CUC", 0.6)]
        [InlineData("USD", "CAD", 1.01)]
        [InlineData("USD", "JPY", 5.55)]
        [InlineData("GBP", "JPY", 2.78)]
        public void Can_Resolve_Exchanges(string from, string to, decimal rate)
        {
            var rates = _rateResolver
                .GetRatesDefinition(new List<(string, string)> {(from, to)}, _availableRates);

            Assert.Equal(rate, rates[(from, to)]);
        }

        [Fact]
        public void Null_Missing_Rates_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _rateResolver.GetRatesDefinition(null, _availableRates));
        }

        [Fact]
        public void Null_Available_Rates_Throws_Exception()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _rateResolver.GetRatesDefinition(new List<(string from, string to)> {("USD", "CAD")}, null));
        }
    }
}
