using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core.UnitOfWork;
using GNB.Infrastructure.Capabilities;
using GNB.Services.Dtos;
using GNB.Services.QuietStone;
using Mapster;
using Microsoft.Extensions.Logging;

namespace GNB.Services
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuietStoneApi _quietStoneApi;
        private readonly ILogger<RateService> _logger;

        public RateService(IUnitOfWork unitOfWork, IQuietStoneApi quietStoneApi, ILogger<RateService> logger)
        {
            _unitOfWork = unitOfWork;
            _quietStoneApi = quietStoneApi;
            _logger = logger;
        }

        public async Task<IEnumerable<RateDto>> GetRates()
        {
            try
            {
                _logger.LogInformation("Attempt to retrieve rates from QuietStone");
                var liveData = await _quietStoneApi.GetRates();
                return liveData.Select(x => x.Adapt<RateDto>());
            }
            catch (GNBException bnbEx)
            {
                _logger.LogError("Error fetching rates from QuietStone. Returning data from DB instead", bnbEx);
                return _unitOfWork.RateRepository.GetAll().Select(x => x.Adapt<RateDto>());
            }
        }
    }
}
