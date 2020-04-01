#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
namespace RESTfulAPITemplate.Core.DTO
{
    public class ProductQueryDTO

#if (RESTFULAPIHELPER)

    : PaginationBase

#endif

    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}