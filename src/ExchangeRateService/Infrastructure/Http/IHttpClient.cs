using System.Threading.Tasks;

namespace ExchangeRateService.Infrastructure.Http
{
    public interface IHttpClient
    {
        Task<TResponse> GetAsync<TResponse>(string endpointUrl);
    }
}