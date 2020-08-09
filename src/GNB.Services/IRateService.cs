using System.Collections.Generic;
using System.Threading.Tasks;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface IRateService
    {
        Task<IEnumerable<RateDto>> GetRates();
    }
}
