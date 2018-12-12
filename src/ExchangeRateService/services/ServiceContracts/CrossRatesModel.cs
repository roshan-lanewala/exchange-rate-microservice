using System;

namespace ExchangeRateService.Services.ServiceContracts
{
    public class CrossRatesModel
    {
        public bool Success { get; set; } = true;
        public string BaseCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Error { get; set; }
    }
}