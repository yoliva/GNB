using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core;
using GNB.Core.UnitOfWork;
using GNB.Infrastructure.Capabilities;
using GNB.Services;
using GNB.Services.Mappings;
using GNB.Services.QuietStone;
using GNB.Services.QuietStone.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GNB.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyNormalizer _normalizer;
        private readonly IQuietStoneApi _failingQuietStoneApi;
        private readonly IQuietStoneApi _quietStoneApi;
        private readonly ILogger<TransactionService> _logger;

        public TransactionServiceTests()
        {
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.TransactionRepository.GetAll())
                .Returns(new List<Transaction>
                {
                    new Transaction {ID = "1", Currency = "USD", Amount = 23.5m, Sku = "AX10"},
                    new Transaction {ID = "2", Currency = "UYU", Amount = 44.00m, Sku = "AX90"},
                    new Transaction {ID = "3", Currency = "CAD", Amount = 10.5m, Sku = "AX15"},
                    new Transaction {ID = "4", Currency = "CAD", Amount = 23m, Sku = "AX10"}
                }.AsQueryable());

            var quietStoneApiMock = new Mock<IQuietStoneApi>();
            quietStoneApiMock.Setup(x => x.GetTransactions())
                .Returns(Task.Run(() => new List<QuietStoneTransactionDto>
                {
                    new QuietStoneTransactionDto {Currency = "USD", Sku = "J385X", Amount = 18m},
                    new QuietStoneTransactionDto {Currency = "CAD", Sku = "J385Y", Amount = 44.5m}
                }.AsEnumerable()));

            var failingQuietStoneApi = new Mock<IQuietStoneApi>();
            failingQuietStoneApi.Setup(x => x.GetTransactions())
                .Returns(Task.Run(() =>
                {
                    throw new GNBException("some fake message", ErrorCode.UnableToRetrieveTransactionsFromQuietStone);
                    return Enumerable.Empty<QuietStoneTransactionDto>();
                }));

            var normalizer = new Mock<ICurrencyNormalizer>();

            _unitOfWork = uowMock.Object;
            _normalizer = normalizer.Object;
            _failingQuietStoneApi = failingQuietStoneApi.Object;
            _quietStoneApi = quietStoneApiMock.Object;
            _logger = new Mock<ILogger<TransactionService>>().Object;

            MapsterConfig.Configure();
        }

        [Fact]
        public async void Transactions_Are_Returned_From_Db_If_QuietStone_Fail()
        {
            var service = new TransactionService(_unitOfWork, _failingQuietStoneApi, _normalizer, _logger);

            var rates = (await service.GetTransactions()).ToList();

            var ax10Transaction = rates.Single(x => x.ID == "4");

            Assert.Equal(4, rates.Count);
            Assert.Equal(23, ax10Transaction.Amount);
            Assert.Equal("CAD", ax10Transaction.Currency);
            Assert.Equal("AX10", ax10Transaction.Sku);
        }

        [Fact(Skip = "Fix when include normalize")]
        public async void Transactions_Are_Filtered_By_Sku()
        {
            var service = new TransactionService(_unitOfWork, _failingQuietStoneApi, _normalizer, _logger);

            var rates = (await service.GetTransactionsBySku("AX10", KnownCurrencies.Usd))
                .ToList();

            var ax10Transaction = rates.Single(x => x.ID == "1");

            Assert.Equal(2, rates.Count);
            Assert.All(rates, x => Assert.Equal("AX10", x.Sku));
            Assert.Equal(23.5m, ax10Transaction.Amount);
            Assert.Equal("USD", ax10Transaction.Currency);
        }

        [Fact]
        public async void Transaction_Are_Returned_From_QuietStone()
        {
            var service = new TransactionService(_unitOfWork, _quietStoneApi, _normalizer, _logger);

            var rates = (await service.GetTransactions()).ToList();

            var j385Transaction = rates.Single(x => x.Sku == "J385Y");

            Assert.Equal(2, rates.Count);
            Assert.Equal(44.5m, j385Transaction.Amount);
            Assert.Equal("CAD", j385Transaction.Currency);
        }

        [Fact(Skip = "Fix when include normalize")]
        public async void Transactions_From_QuietStone_Are_Filtered_By_Sku()
        {
            var service = new TransactionService(_unitOfWork, _quietStoneApi, _normalizer, _logger);

            var rates = (await service.GetTransactionsBySku("J385X", KnownCurrencies.Usd))
                .ToList();

            var j385Transaction = rates.Single(x => x.Sku == "J385X");

            Assert.Single(rates);
            Assert.All(rates, x => Assert.Equal("J385X", x.Sku));
            Assert.Equal(18, j385Transaction.Amount);
            Assert.Equal("USD", j385Transaction.Currency);
        }
    }
}
