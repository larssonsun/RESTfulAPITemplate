using System;
using Microsoft.Extensions.DependencyInjection;
using RESTfulAPISample.Core.Interface;
using RESTfulAPISample.Core.Shaping;
using RESTfulAPISample.Core.SortAndQuery;

namespace RESTfulAPISample.Core
{
    public static class RESTfulAPIHelperExtensions
    {
        public static void AddRESTfulAPIHelper(this IServiceCollection services, Action<RESTfulAPIHelperOptions> setupAction = null)
        {
            var options = new RESTfulAPIHelperOptions();
            setupAction?.Invoke(options);
            var propertyMappingContainer = new PropertyMappingContainer(options);

            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }
    }
}