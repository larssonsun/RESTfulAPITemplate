using Microsoft.Extensions.DependencyInjection;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Api.Extension
{
    public static class PropertyMappingExtensions
    {
        public static void AddPropertyMappings(this IServiceCollection services)
        {
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<ProductPropertyMapping>();

            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            // services.AddTransient<ITypeHelperService, TypeHelperService>();
        }
    }
}