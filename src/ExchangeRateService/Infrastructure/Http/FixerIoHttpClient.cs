using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeRateService.Infrastructure.Http
{
    public class FixerIoHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public FixerIoHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("fixerIo");
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpointUrl)
        {
            try
            {
            var request = new HttpRequestMessage(HttpMethod.Get, endpointUrl);

            var response = await _httpClient.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            return default(TResponse);
            }
            catch(Exception ex)
            {
                return default(TResponse);
            }
        }
    }
}