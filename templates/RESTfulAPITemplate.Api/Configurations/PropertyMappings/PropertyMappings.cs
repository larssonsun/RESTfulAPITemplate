using AutoMapper;
using RESTfulAPITemplate.App.Model;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.Core.Specification.Filter;

namespace RESTfulAPITemplate.App.Configuration.PropertyMapping
{
    public class PropertyMappings : Profile
    {
        public PropertyMappings()
        {

#if (ENABLEJWTAUTHENTICATION)

            CreateMap<LoginCommandDTO, LoginCommand>();

            // CreateMap<LoginCommand, UserLogin>()
            //     .ConstructUsing(c => new UserLogin(c.Username, c.Password, null, null, null));

#endif

            CreateMap<ProductFilterDTO, ProductFilter>();
            CreateMap<Product, ProductDTO>().ForMember(
                dto => dto.FullName,
                opt => opt.MapFrom(entity => $"{entity.Name} {entity.Description}"));
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
        }
    }
}
