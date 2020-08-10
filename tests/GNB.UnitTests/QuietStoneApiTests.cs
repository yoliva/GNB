using System.Linq;
using GNB.QuietStone;
using GNB.Services.Mappings;
using GNB.Services.QuietStone;
using Microsoft.Extensions.Options;
using Xunit;

namespace GNB.UnitTests
{
    public class QuietStoneApiTests
    {
        private readonly IQuietStoneApi _quietStoneApi;
        public QuietStoneApiTests()
        {
            var opt = Options.Create(new QuietStoneConfig
            {
                BaseUrl = "http://quiet-stone-2094.herokuapp.com/",
                TransactionEndpoint = "/transactions.json",
                RatesEndpoint = "/rates.json"
            });

            _quietStoneApi = new QuietStoneApi(opt);

            MapsterConfig.Configure();
        }

        [Fact(Skip = "just for development purposes")]
        public async void Can_Fetch_Rates()
        {
            var rates = await _quietStoneApi.GetRates();

            Assert.True(rates.Any());
        }

        [Fact(Skip = "just for development purposes")]
        public async void Can_Fetch_Transactions()
        {
            var transactions = await _quietStoneApi.GetTransactions();

            Assert.True(transactions.Any());
        }
    }
}
