using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GNB.Core.UnitOfWork;
using GNB.Infrastructure.Capabilities;
using GNB.Services.Dtos;
using GNB.Services.QuietStone;
using Mapster;

namespace GNB.Services
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuietStoneApi _quietStoneApi;

        public RateService(IUnitOfWork unitOfWork, IQuietStoneApi quietStoneApi)
        {
            _unitOfWork = unitOfWork;
            _quietStoneApi = quietStoneApi;
        }

        public async Task<IEnumerable<RateDto>> GetRates()
        {
            try
            {
                var liveData = await _quietStoneApi.GetRates();
                return liveData.Select(x => x.Adapt<RateDto>());
            }
            catch (GNBException bnbEx)
            {
                return _unitOfWork.RateRepository.GetAll().Select(x => x.Adapt<RateDto>());
            }
        }
    }
}
