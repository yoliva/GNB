using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GNB.Infrastructure.Capabilities;
using GNB.Services.QuietStone.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GNB.Services.QuietStone
{
    public class QuietStoneApi : IQuietStoneApi
    {
        private readonly Lazy<HttpClient> _client;
        private readonly QuietStoneConfig _cfg;

        public QuietStoneApi(IOptions<QuietStoneConfig> cfg)
        {
            _cfg = cfg.Value;
            _client = new Lazy<HttpClient>(() => new HttpClient {BaseAddress = new Uri(_cfg.BaseUrl)});
        }

        public async Task<IEnumerable<QuietStoneRateDto>> GetRates()
        {
            var response = await _client.Value.GetAsync(_cfg.RatesEndpoint);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                const string msg = "Unable to retrieve rates from QuietStone";
                throw new GNBException(msg, ErrorCode.UnableToRetrieveRatesFromQuietStone);
            }

            var dataStr = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<QuietStoneRateDto>>(dataStr);
        }
    }
}
