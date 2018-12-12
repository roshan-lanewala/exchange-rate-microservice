using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateService.Tests.Integration.Setup;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateService.Tests.Integration.ExchangeRates
{
    [TestFixture]
    public class ExchangeRatesTests
    {
        [Test]
        [TestCase("{\"success\":true,\"timestamp\":1544485751,\"base\":\"EUR\",\"date\":\"2018-12-11\",\"rates\":{\"AUD\":1.579088,\"SEK\":10.326085,\"USD\":1.135667,\"GBP\":0.903954,\"EUR\":1}}"
            , "{\"success\":true,\"baseCurrency\":\"AUD\",\"targetCurrency\":\"USD\",\"exchangeRate\":0.7191917106583040337207299403,\"timestamp\":\"2018-12-10T23:49:11+00:00\"}")]
        public async Task GivenACallToGetLatestRates_RatesAreReturned(string rates, string expectedResult)
        {
            using (var server = new FixerIoApiServerFixture().WithValidExchangeRatesResponse(rates))
            {
                var httpClient = new HttpClient { BaseAddress = new Uri(server.Url) };
                var response = await httpClient.GetAsync("api?basecurrency=AUD&targetcurrency=USD");
                response.StatusCode.Should().Be(200);
                var content = await response.Content.ReadAsStringAsync();
                content.Should().BeEquivalentTo(expectedResult);
            }
        }
    }

    public class FixerIoApiServerFixture : ServerFixture
    {
        public FixerIoApiServerFixture WithValidExchangeRatesResponse(string response)
        {
            A.CallTo(() => this.FakeFixerIoApiService.GetLatest()).Returns(response);
            return this;
        } 
    }
}