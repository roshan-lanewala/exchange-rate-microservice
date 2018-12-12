using System.ComponentModel.DataAnnotations;

namespace ExchangeRateService.Features.ExchangesRates.DataContracts
{
    public class RatesRequest
    {
        [Required(AllowEmptyStrings=false)]
        public string BaseCurrency { get; set; }
        [Required(AllowEmptyStrings=false)]
        public string TargetCurrency { get; set; }
    }
}