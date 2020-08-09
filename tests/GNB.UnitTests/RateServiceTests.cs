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
    public class RateServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;

        public RateServiceTests()
        {
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.RateRepository.GetAll())
                .Returns(new List<Rate>
                {
                    new Rate {ID = "1", To = "USD", From = "CAD", ChangeRate = 23.5m},
                    new Rate {ID = "2", To = "UYU", From = "USD", ChangeRate = 44.00m},
                    new Rate {ID = "3", To = "CAD", From = "USD", ChangeRate = 10.5m}
                }.AsQueryable());

            _unitOfWork = uowMock.Object;

            MapsterConfig.Configure();
        }

        [Fact]
        public void RatesAreReturned()
        {
            var service = new RateService(_unitOfWork);
            
            var rates = service.GetRates().ToList();

            var usdToCad = rates.Single(x => x.From == "USD" && x.To == "CAD");

            Assert.Equal(3, rates.Count);
            Assert.Equal(10.5m, usdToCad.Rate);
        }
    }
}
