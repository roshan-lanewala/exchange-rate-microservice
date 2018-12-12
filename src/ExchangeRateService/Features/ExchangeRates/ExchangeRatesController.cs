using System.Threading.Tasks;
using ExchangeRateService.Features.ExchangesRates.DataContracts;
using ExchangeRateService.Features.ExchangesRates.Extensions;
using ExchangeRateService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateService.Features.ExchangesRates
{
    [Route("/api")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IFixerIoService _fixerIoService;
        public ExchangeRatesController(IFixerIoService fixerIoService)
        {
            _fixerIoService = fixerIoService;
        }
        
        [HttpGet]
        public async Task<ActionResult<RatesResponse>> Get([FromQuery] RatesRequest request)
        {
            if(!ModelState.IsValid)
            {
                return await Task.FromResult(BadRequest(ModelState));
            }
            
            var result = await _fixerIoService.GetLatestRatesAsync(request.BaseCurrency, request.TargetCurrency);
            
            return Ok(result.ToRatesResponse(request.BaseCurrency, request.TargetCurrency));
        }
    }
}