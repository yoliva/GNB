using System.Collections.Generic;
using System.Linq;
using GNB.Core;
using GNB.Core.UnitOfWork;
using GNB.Services;
using GNB.Services.Mappings;
using Moq;
using Xunit;

namespace GNB.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;

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

            _unitOfWork = uowMock.Object;

            MapsterConfig.Configure();
        }

        [Fact]
        public void TransactionsAreReturned()
        {
            var service = new TransactionService(_unitOfWork);

            var rates = service.GetTransactions().ToList();

            var ax10Transaction = rates.Single(x => x.ID == "4");

            Assert.Equal(4, rates.Count);
            Assert.Equal(23, ax10Transaction.Amount);
            Assert.Equal("CAD", ax10Transaction.Currency);
            Assert.Equal("AX10", ax10Transaction.Sku);
        }

        [Fact]
        public void TransactionsAreFilteredBySku()
        {
            var service = new TransactionService(_unitOfWork);

            var rates = service.GetTransactionsBySku("AX10").ToList();

            var ax10Transaction = rates.Single(x => x.ID == "1");

            Assert.Equal(2, rates.Count);
            Assert.All(rates, x => Assert.Equal("AX10", x.Sku));
            Assert.Equal(23.5m, ax10Transaction.Amount);
            Assert.Equal("USD", ax10Transaction.Currency);
        }
    }
}
