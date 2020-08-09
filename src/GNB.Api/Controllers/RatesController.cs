using System.Threading.Tasks;
using GNB.Services;
using Microsoft.AspNetCore.Mvc;

namespace GNB.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IRateService _rateService;

        public RatesController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _rateService.GetRates());
        }
    }
}
