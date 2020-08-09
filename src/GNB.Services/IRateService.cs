using System.Collections.Generic;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface IRateService
    {
        IEnumerable<RateDto> GetRates();
    }
}
