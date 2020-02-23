using Microsoft.Extensions.DependencyInjection;
using RESTfulAPISample.Core.Interface;
using RESTfulAPISample.Core.Shaping;
using RESTfulAPISample.Core.SortAndQuery;

namespace RESTfulAPISample.Core
{
    public static class PropertyMappingExtensions
    {
        public static void AddPropertyMappings<TPropertyMapping>(this IServiceCollection services)
        where TPropertyMapping : IPropertyMapping, new()
        {
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<TPropertyMapping>();

            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }
    }
}