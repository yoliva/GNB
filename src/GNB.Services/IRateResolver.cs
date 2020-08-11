using System.Collections.Generic;
using GNB.Services.Dtos;

namespace GNB.Services
{
    public interface IRateResolver
    {
        Dictionary<(string from, string to), decimal> GetRatesDefinition(List<(string from, string to)> missingRates,
            List<RateDto> availableRates);
    }
}
