using AutoMapper;
using RESTfulAPISample.Api.DTO;
using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Api.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

#if (ENABLEJWTAUTHENTICATION)

            CreateMap<LoginRequestDTO, LoginRequest>();

#endif

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductCreateOrUpdateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
        }
    }
}
