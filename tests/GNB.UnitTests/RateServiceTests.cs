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
    public class RateServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IQuietStoneApi> _quietStoneApi;
        private readonly ILogger<RateService> _logger;

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

            var quietStoneApiMock = new Mock<IQuietStoneApi>();
            quietStoneApiMock.Setup(x => x.GetRates())
                .Returns(Task.Run(() => new List<QuietStoneRateDto>
                {
                    new QuietStoneRateDto {To = "USD", From = "CAD", Rate = 50.5m},
                    new QuietStoneRateDto {To = "CAD", From = "USD", Rate = 44.5m}
                }.AsEnumerable()));

            _unitOfWork = uowMock.Object;
            _quietStoneApi = quietStoneApiMock;
            _logger = new Mock<ILogger<RateService>>().Object;

            MapsterConfig.Configure();
        }

        [Fact]
        public async void Rates_Are_From_DB_If_QuietStone_Fail_To_Return_Rates()
        {
            _quietStoneApi.Setup(x => x.GetRates())
                .Returns(Task.Run(() =>
                {
                    throw new GNBException("some fake message", ErrorCode.UnableToRetrieveRatesFromQuietStone);
                    return Enumerable.Empty<QuietStoneRateDto>();
                }));

            var service = new RateService(_unitOfWork, _quietStoneApi.Object, _logger);
            
            var rates = (await service.GetRates()).ToList();

            var usdToCad = rates.Single(x => x.From == "USD" && x.To == "CAD");

            Assert.Equal(3, rates.Count);
            Assert.Equal(10.5m, usdToCad.Rate);
        }

        [Fact]
        public async void Rates_Are_From_Properly_From_QuietStone()
        {
            var service = new RateService(_unitOfWork, _quietStoneApi.Object, _logger);

            var rates = (await service.GetRates()).ToList();

            var usdToCad = rates.Single(x => x.From == "USD" && x.To == "CAD");

            Assert.Equal(2, rates.Count);
            Assert.Equal(44.5m, usdToCad.Rate);
        }
    }
}
