using System;
using Microsoft.Extensions.DependencyInjection;
using Larsson.RESTfulAPIHelper.Interface;
using Larsson.RESTfulAPIHelper.Shaping;
using Larsson.RESTfulAPIHelper.SortAndQuery;

namespace Larsson.RESTfulAPIHelper
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