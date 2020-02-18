using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Core.DomainModel
{
    public class PaginationBase
    {
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 0;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public string OrderBy { get; set; } = nameof(IEntity.Id);
        public string Fields { get; set; }
        protected int MaxPageSize { get; set; } = 100;
    }
}