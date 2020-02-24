using System.Collections.Generic;
using Larsson.RESTfulAPIHelper.Pagination;
#if (DISTRIBUTEDCACHE)
using MessagePack;
#endif

namespace RESTfulAPISample.Core.DomainModel
{
#if (DISTRIBUTEDCACHE)
    [MessagePackObject(keyAsPropertyName: true)]
#endif
    public class PagedListBase<T> : PaginatedList<T> where T : class
    {
        public PagedListBase(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data) : base(pageIndex, pageSize, totalItemsCount, data)
        {
        }
    }
}