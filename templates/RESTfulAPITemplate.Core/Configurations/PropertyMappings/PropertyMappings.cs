using AutoMapper;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Configuration.PropertyMapping
{
    public class PropertyMappings : Profile
    {
        public PropertyMappings()
        {

#if (ENABLEJWTAUTHENTICATION)

            CreateMap<LoginRequestDTO, LoginRequest>();

#endif
            CreateMap<ProductQueryDTO, ProductQuery>();
            CreateMap<Product, ProductDTO>().ForMember(
                dto => dto.FullName,
                opt => opt.MapFrom(entity => $"{entity.Name} {entity.Description}"));
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
        }
    }
}
