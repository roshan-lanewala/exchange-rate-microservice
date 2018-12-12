using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateService.Tests.Integration.Setup
{
    public class DependencyServiceStartupSetting
    {        
        public Action<IServiceCollection> ConfigureServices { get; set; }
        public IEnumerable<Type> ControllerTypes { get; set; }
    }
}
