using System.Collections.Generic;
#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Pagination;
#endif
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPISample.Core.DomainModel
{
#if (DISTRIBUTEDCACHE)

    [MessagePackObject(keyAsPropertyName: true)]

#endif

#if (RESTFULAPIHELPER)

    public class PagedListBase<T> : PaginatedList<T> where T : class
    {
        public PagedListBase(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data) : base(pageIndex, pageSize, totalItemsCount, data)
        {
        }
    }

#else

    public interface PagedListBase<T> : IEnumerable<T>
    {

    }

#endif
}