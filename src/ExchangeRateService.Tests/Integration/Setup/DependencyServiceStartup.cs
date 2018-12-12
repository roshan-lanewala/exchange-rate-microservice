using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateService.Tests.Integration.Setup
{
    public class DependencyServiceStartup
    {
        private readonly DependencyServiceStartupSetting _serviceSettings;

        public DependencyServiceStartup(DependencyServiceStartupSetting serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(DependencyServiceStartup));

            services.AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
              .AddApplicationPart(assembly);

            var restrictionProvider = new ControllerFilter(RestrictionType.Whitelist, _serviceSettings.ControllerTypes);
            restrictionProvider.InjectInto(services);

            _serviceSettings?.ConfigureServices?.Invoke(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

    }
}