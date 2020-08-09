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
        public ActionResult Get()
        {
            return Ok(_rateService.GetRates());
        }
    }
}
