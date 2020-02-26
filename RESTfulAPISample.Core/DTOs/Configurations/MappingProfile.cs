using AutoMapper;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Entity;

namespace RESTfulAPISample.Core.DTO.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

#if (ENABLEJWTAUTHENTICATION)

            CreateMap<LoginRequestDTO, LoginRequest>();

#endif

            CreateMap<Product, ProductDTO>().ForMember(
                dto => dto.FullName,
                opt => opt.MapFrom(entity => $"{entity.Name} {entity.Description}"));
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
        }
    }
}
