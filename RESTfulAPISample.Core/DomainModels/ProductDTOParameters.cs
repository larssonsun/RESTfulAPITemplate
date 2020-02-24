using Larsson.RESTfulAPIHelper.Pagination;

namespace RESTfulAPISample.Core.DomainModel
{
    public class ProductDTOParameters : PaginationBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}