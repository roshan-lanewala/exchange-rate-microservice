using System.Linq;

namespace ExchangeRateService.Services.ServiceContracts
{
    public static class Symbols
    {
        public const string AUD = "AUD";
        public const string SEK = "SEK";
        public const string USD = "USD";
        public const string GBP = "GBP";
        public const string EUR = "EUR";

        public static bool IsValid(string symbol)
        {
            return ValidSymbols.Contains(symbol);
        }

        private static string[] ValidSymbols = new string[]
        {
            AUD,
            SEK,
            USD,
            GBP,
            EUR,
        };
    }
}