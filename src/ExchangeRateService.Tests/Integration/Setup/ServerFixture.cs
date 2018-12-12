using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using ExchangeRateService.Tests.Integration.Setup.MockedService;
using FakeItEasy;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateService.Tests.Integration.Setup
{
    public class ServerFixture : IDisposable
    {
        public string Url { get; private set; }
        private IWebHost _fixerIoApiServiceHost;
        private IWebHost _server;

        public HttpClient HttpClient { get; private set; }
        protected IFixerIOApiService FakeFixerIoApiService { get; } = A.Fake<IFixerIOApiService>();

        public ServerFixture()
        {
            StartServer();
        }

        private void StartServer()
        {
            var fixerIoApiServiceStartupSetting = new DependencyServiceStartupSetting
            {
                ConfigureServices = s => s.AddSingleton<IFixerIOApiService>(c => FakeFixerIoApiService),
                ControllerTypes = new List<Type> { typeof(FixerIOAPIController) }
            };

            var fixerIoUrl = $"http://localhost:{PortHelper.GetAvailablePort()}";
            _fixerIoApiServiceHost = StartDependencyServiceHost(fixerIoUrl, fixerIoApiServiceStartupSetting);

            Url = $"http://localhost:{PortHelper.GetAvailablePort()}";
            _server = StartMainHost(fixerIoUrl);

            HttpClient = new HttpClient { BaseAddress = new Uri(Url) };
        }

        private IWebHost StartDependencyServiceHost(string fixerApiUrl, DependencyServiceStartupSetting dependencyServiceStartupSetting)
        {
            var host = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(dependencyServiceStartupSetting);
                })
                .UseKestrel()
                .UseUrls(fixerApiUrl)
                .UseStartup<DependencyServiceStartup>()
                .Build();

            host.Start();
            return host;
        }

        private IWebHost StartMainHost(string fixerApiUrl)
        {
            var host = WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    var configurationOverrides = new Dictionary<string, string>
                    {
                        ["fixer-Api-Base-Address"] = $"{fixerApiUrl}/api",
                        ["APIKey"] = "access_key_test"
                    };
                    configuration.SetBasePath(Directory.GetCurrentDirectory())
                        .AddInMemoryCollection(configurationOverrides);
                })
                .UseKestrel()
                .UseUrls(Url)
                .UseStartup<Startup>()
                .Build();

            host.Start();
            return host;
        }

        private void StopHost(IWebHost host)
        {
            try
            {
                host?.StopAsync(TimeSpan.FromSeconds(5)).ContinueWith(t => host?.Dispose());
            }
            catch (Exception) { }
        }

        public void Dispose()
        {
            StopHost(_fixerIoApiServiceHost);
            _fixerIoApiServiceHost = null;

            StopHost(_server);
            _server = null;
        }
    }
}