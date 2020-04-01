using System.Collections.Generic;
#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPITemplate.Core.DomainModel
{


#if (RESTFULAPIHELPER)

    #if (DISTRIBUTEDCACHE)

    [MessagePackObject(keyAsPropertyName: true)]

    #endif

    public class PagedListBase<T> : PaginatedList<T> where T : class
    {
        public PagedListBase(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data) : base(pageIndex, pageSize, totalItemsCount, data)
        {
        }
    }

#endif
}