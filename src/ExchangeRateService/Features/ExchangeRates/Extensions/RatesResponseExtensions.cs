using ExchangeRateService.Features.ExchangesRates.DataContracts;
using ExchangeRateService.Services.ServiceContracts;

namespace ExchangeRateService.Features.ExchangesRates.Extensions
{
    public static class RatesResponseExtensions
    {
        public static RatesResponse ToRatesResponse(this CrossRatesModel model, string baseCurrency, string targetCurrency)
        {
            if (model.Success) {
                return new RatesResponse {
                    BaseCurrency = baseCurrency,
                    TargetCurrency = targetCurrency,
                    ExchangeRate = model.ExchangeRate,
                    Timestamp = model.Timestamp,
                };
            }
            else {
                return new RatesResponse {
                    Success = false,
                    Error = model.Error
                };
            }
        }
    }
}