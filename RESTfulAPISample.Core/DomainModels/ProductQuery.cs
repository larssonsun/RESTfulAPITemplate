#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
namespace RESTfulAPISample.Core.DomainModel
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