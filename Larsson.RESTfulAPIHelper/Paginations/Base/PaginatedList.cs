
using System.Collections.Generic;

namespace Larsson.RESTfulAPIHelper.Pagination
{
    public class PaginatedList<T> : List<T> where T : class
    {
        public PaginationBase PaginationBase { get; }

        public int TotalItemsCount { get; set; }
        public int PageCount => TotalItemsCount / PaginationBase.PageSize + (TotalItemsCount % PaginationBase.PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PaginationBase.PageIndex > 0;
        public bool HasNext => PaginationBase.PageIndex < PageCount - 1;

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedList(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;

            PaginationBase = new PaginationBase
            {
                PageIndex = PageIndex,
                PageSize = PageSize
            };
            TotalItemsCount = totalItemsCount;
            AddRange(Data);
        }
    }
}