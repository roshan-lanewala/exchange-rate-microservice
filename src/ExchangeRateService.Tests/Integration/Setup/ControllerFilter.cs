﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateService.Tests.Integration.Setup
{
    public enum RestrictionType
    {
        Whitelist,
        Blacklist
    }

    public class ControllerFilter : ControllerFeatureProvider
    {
        private readonly RestrictionType _type;
        private readonly IEnumerable<TypeInfo> _controllers;

        public ControllerFilter(RestrictionType type, IEnumerable<Type> controllers)
        {
            _type = type;
            _controllers = controllers.Select(c => c.GetTypeInfo());
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            if (typeInfo.Name.Contains("Controller"))
            {
                Console.WriteLine($"Found {typeInfo.Name}");
            }

            if (_type == RestrictionType.Whitelist)
            {
                if (!_controllers.Contains(typeInfo))
                {
                    return false;
                }
            }
            else if (_type == RestrictionType.Blacklist)
            {
                if (_controllers.Contains(typeInfo))
                {
                    return false;
                }
            }
            return base.IsController(typeInfo);
        }

        public bool InjectInto(IServiceCollection services)
        {
            var managerService = services.FirstOrDefault(s => s.ServiceType == typeof(ApplicationPartManager));
            if (managerService != null)
            {
                var providers = ((ApplicationPartManager)managerService.ImplementationInstance).FeatureProviders;
                var provider = providers.FirstOrDefault(p => p.GetType() == typeof(ControllerFeatureProvider));
                if (provider != null)
                {
                    providers.Remove(provider);
                    providers.Add(this);
                    return true;
                }
            }
            return false;
        }
    }
}