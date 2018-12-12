using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateService.Features.ExchangesRates.DataContracts
{
    public class RatesResponse
    {
        public bool Success { get; set; } = true;
        public string Error { get; set; }
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        [DisplayFormat(DataFormatString="{0:#.#####")]
        public decimal ExchangeRate { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}