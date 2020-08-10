using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core;
using GNB.Core.UnitOfWork;
using GNB.Infrastructure.Capabilities;
using GNB.Services;
using GNB.Services.Dtos;
using GNB.Services.Mappings;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GNB.UnitTests
{
    public class TransactionServiceTests
    {
        private const decimal CadToUsdRate = 1.5m;
        private readonly Transaction _x10CadTransaction;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyNormalizer _normalizer;
        private readonly ITransactionDataProvider _failingTransactionDataProvider;
        private readonly ITransactionDataProvider _transactionDataProvider;
        private readonly ILogger<TransactionService> _logger;

        public TransactionServiceTests()
        {
            _x10CadTransaction = new Transaction {ID = "4", Currency = KnownCurrencies.CAD, Amount = 23m, Sku = "AX10"};
            var x10UsdTransactionDto = new TransactionDto {ID = "1", Currency = KnownCurrencies.USD, Amount = 23.5m, Sku = "AX10"};

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.TransactionRepository.GetAll())
                .Returns(new List<Transaction>
                {
                    new Transaction {ID = "1", Currency = KnownCurrencies.USD, Amount = 23.5m, Sku = "AX10"},
                    new Transaction {ID = "2", Currency = KnownCurrencies.UYU, Amount = 44.00m, Sku = "AX90"},
                    new Transaction {ID = "3", Currency = KnownCurrencies.CAD, Amount = 10.5m, Sku = "AX15"},
                    _x10CadTransaction
                }.AsQueryable());

            var transactionDataProvider = new Mock<ITransactionDataProvider>();
            transactionDataProvider.Setup(x => x.GetTransactions())
                .Returns(Task.Run(() => new List<Transaction>
                {
                    new Transaction {Currency = KnownCurrencies.USD, Sku = "J385X", Amount = 18m},
                    new Transaction {Currency = KnownCurrencies.CAD, Sku = "J385Y", Amount = 44.5m}
                }));

            var failingTransactionDataProvider = new Mock<ITransactionDataProvider>();
            failingTransactionDataProvider.Setup(x => x.GetTransactions())
                .Returns(Task.Run(() =>
                {
                    throw new GNBException("some fake message", ErrorCode.UnableToRetrieveTransactionsFromQuietStone);
                    return new List<Transaction>();
                }));

            var normalizer = new Mock<ICurrencyNormalizer>();
            normalizer.Setup(x => x.Normalize(KnownCurrencies.USD, It.IsAny<IEnumerable<TransactionDto>>()))
                .Returns(Task.Run(() =>
                    new List<TransactionDto>
                    {
                        new TransactionDto
                        {
                            ID = _x10CadTransaction.ID,
                            Currency = KnownCurrencies.USD,
                            Sku = _x10CadTransaction.Sku,
                            Amount = _x10CadTransaction.Amount * CadToUsdRate
                        },
                        x10UsdTransactionDto
                    }.AsEnumerable()));

            _unitOfWork = uowMock.Object;
            _normalizer = normalizer.Object;
            _failingTransactionDataProvider = failingTransactionDataProvider.Object;
            _transactionDataProvider = transactionDataProvider.Object;
            _logger = new Mock<ILogger<TransactionService>>().Object;

            MapsterConfig.Configure();
        }

        [Fact]
        public async void Transactions_Are_Returned_From_Db_If_QuietStone_Fail()
        {
            var service = new TransactionService(_unitOfWork, _failingTransactionDataProvider, _normalizer, _logger);

            var transactions = (await service.GetTransactions()).ToList();

            var ax10Transaction = transactions.Single(x => x.ID == "4");

            Assert.Equal(4, transactions.Count);
            Assert.Equal(23, ax10Transaction.Amount);
            Assert.Equal(KnownCurrencies.CAD, ax10Transaction.Currency);
            Assert.Equal("AX10", ax10Transaction.Sku);
        }

        [Fact]
        public async void Transactions_Are_Filtered_By_Sku()
        {
            var service = new TransactionService(_unitOfWork, _failingTransactionDataProvider, _normalizer, _logger);

            var transactions = (await service.GetTransactionsBySku("AX10", KnownCurrencies.USD))
                .ToList();

            var ax10Transaction = transactions.Single(x => x.ID == "4");

            Assert.Equal(2, transactions.Count);
            Assert.All(transactions, x => Assert.Equal("AX10", x.Sku));
            Assert.Equal(_x10CadTransaction.Amount * CadToUsdRate, ax10Transaction.Amount);
            Assert.Equal(KnownCurrencies.USD, ax10Transaction.Currency);
        }

        [Fact]
        public async void Transaction_Are_Returned_From_QuietStone()
        {
            var service = new TransactionService(_unitOfWork, _transactionDataProvider, _normalizer, _logger);

            var transactions = (await service.GetTransactions()).ToList();

            var j385Transaction = transactions.Single(x => x.Sku == "J385Y");

            Assert.Equal(2, transactions.Count);
            Assert.Equal(44.5m, j385Transaction.Amount);
            Assert.Equal(KnownCurrencies.CAD, j385Transaction.Currency);
        }
    }
}
