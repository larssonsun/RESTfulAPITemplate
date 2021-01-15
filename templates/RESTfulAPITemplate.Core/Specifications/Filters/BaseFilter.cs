#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif

namespace RESTfulAPITemplate.Core.Specification.Filter
{
    public class BaseFilter

#if (RESTFULAPIHELPER)

: PaginationBase

#endif
    { }
}