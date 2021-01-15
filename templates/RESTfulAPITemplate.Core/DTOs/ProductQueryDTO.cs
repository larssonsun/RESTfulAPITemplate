#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
namespace RESTfulAPITemplate.Core.DTO
{
    public class ProductFilterDTO

#if (RESTFULAPIHELPER)

    : PaginationBase
#else

    : BaseFilterDTO

#endif

    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}