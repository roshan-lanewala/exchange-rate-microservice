using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateService.Tests.Integration.Setup.MockedService
{
    public interface IFixerIOApiService
    {
        Task<string> GetLatest();
    }

    [Route("")]
    public class FixerIOAPIController : ControllerBase
    {
        private readonly IFixerIOApiService _fixerIoApiService;

        public FixerIOAPIController(IFixerIOApiService fixerIoApiService)
        {
            _fixerIoApiService = fixerIoApiService;            
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatest([FromQuery] string accessKey, [FromQuery]string symbols)
        {
            return Ok(await _fixerIoApiService.GetLatest());
        }
    }
}