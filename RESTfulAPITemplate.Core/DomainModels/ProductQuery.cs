#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
namespace RESTfulAPITemplate.Core.DomainModel
{
    public class ProductQuery

#if (RESTFULAPIHELPER)

    : PaginationBase

#endif

    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}