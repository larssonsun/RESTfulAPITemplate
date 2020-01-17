using AutoMapper;
using RESTfulAPISample.Api.Resource;
using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Api.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResource>();
            CreateMap<ProductAddResource, Product>();
        }
    }
}
