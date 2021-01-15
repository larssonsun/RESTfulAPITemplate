#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif

namespace RESTfulAPITemplate.Core.Specification.Filter
{
    public class BaseFilter

#if (RESTFULAPIHELPER)

: PaginationBase

#endif
    {
#if (!RESTFULAPIHELPER)

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }

#endif
    }
}