using System;
using System.Threading.Tasks;
using ExchangeRateService.Infrastructure.Http;
using ExchangeRateService.Services.Extensions;
using ExchangeRateService.Services.ServiceContracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateService.Services
{

    public interface IFixerIoService
    {
        Task<CrossRatesModel> GetLatestRatesAsync(string baseCurrency, string targetCurrency);
    }

    public class FixerIoService : IFixerIoService
    {
        private const int MAX_CACHE_EXPIRATION_MINUTES = 60;
        private readonly string _apiKey;
        private readonly IMemoryCache _cache;
        private readonly IHttpClient _fixerIoHttpClient;
        public FixerIoService(IMemoryCache cache, IHttpClient fixerIoHttpClient, IConfiguration configuration)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _fixerIoHttpClient = fixerIoHttpClient ?? throw new ArgumentNullException(nameof(fixerIoHttpClient));
            _apiKey = configuration["APIKey"] 
                ?? throw new System.ApplicationException("FixerIo Api key configuration missing.");
        }

        public async Task<CrossRatesModel> GetLatestRatesAsync(string baseCurrency, string targetCurrency)
        {
            if(!Symbols.IsValid(baseCurrency) || !Symbols.IsValid(targetCurrency))
            {
                return new CrossRatesModel {
                    Success = false,
                    Error = $"Invalid value for {nameof(baseCurrency)} and/or {nameof(targetCurrency)}"
                };
            }
            _cache.TryGetValue("latest-rates-eur-base", out CrossRatesModel latestRates);
            if(latestRates != null) {
                return latestRates;
            }

            var symbols = new [] { Symbols.AUD, Symbols.SEK, Symbols.USD, Symbols.GBP, Symbols.EUR };
            var endpointUrl = $"latest?access_key={_apiKey}&symbols={string.Join(',', symbols)}";
            var response = (await _fixerIoHttpClient.GetAsync<ExchangeRateModel>(endpointUrl))
                .ToCrossRates(baseCurrency, targetCurrency);

            if(response?.Success ?? false) {
                var expiration = DateTimeOffset.Now.ToUnixTimeSeconds() - response.Timestamp.ToUnixTimeSeconds();
                _cache.Set("latest-rates-eur-base", response,
                    (expiration <= 0 
                        ? TimeSpan.FromMinutes(60) 
                        : TimeSpan.FromSeconds(expiration)));
            }
            return response != null
                ? response
                : new CrossRatesModel {
                    Success = false,
                    Error = "Unable to retrieve exchange rates."
                };
        }
    }
}