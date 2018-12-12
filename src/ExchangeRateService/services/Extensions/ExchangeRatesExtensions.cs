using System;
using ExchangeRateService.Services.ServiceContracts;

namespace ExchangeRateService.Services.Extensions
{
    public static class ExchangeRatesExtensions
    {
        public static CrossRatesModel ToCrossRates(this ExchangeRateModel @this, string baseCurrency, string targetCurrency)
        {
            if (!@this.Rates.TryGetValue(Symbols.EUR, out var baseEURRate)) {
                return new CrossRatesModel {
                    Success = false,
                    Error = "Unable to retrieve Base Rate."
                };
            }
            if (!@this.Rates.TryGetValue(baseCurrency, out var requestedBaseCurrencyRates)) {
                return new CrossRatesModel {
                    Success = false,
                    Error = "Unable to retrieve rates for requested base currency."
                };
            }
            if (!@this.Rates.TryGetValue(targetCurrency, out var targetCurrencyRates)) {
                return new CrossRatesModel {
                    Success = false,
                    Error = "Unable to retrieve rates for target currency."
                };
            }
            
            var calculatedRates = targetCurrencyRates / requestedBaseCurrencyRates;
            return new CrossRatesModel {
                Success = @this.Success,
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                ExchangeRate = calculatedRates,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(@this.Timestamp),
                Error = !@this.Success ? @this.Error.Info : null,
            };
        }
    }
}