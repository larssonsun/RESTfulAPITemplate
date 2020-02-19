using Microsoft.Extensions.DependencyInjection;
using RESTfulAPISample.Core.Interface;
using RESTfulAPISample.Core.Shaping;
using RESTfulAPISample.Core.SortAndQuery;

namespace RESTfulAPISample.Core
{
    public static class PropertyMappingExtensions
    {
        public static void AddPropertyMappings(this IServiceCollection services)
        {
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<ProductPropertyMapping>();

            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }
    }
}