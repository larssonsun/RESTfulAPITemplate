using Ardalis.Specification;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.Core.Specification.Filter;

namespace RESTfulAPITemplate.Core.Specification
{
    public class ProductSpec : Specification<Product>
    {
        public ProductSpec(ProductFilter filter)
        {
            if (filter.PageSize > 0)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.Name))
                Query.Where(x => x.Name == filter.Name);

            if (!string.IsNullOrEmpty(filter.Description))
                Query.Where(x => x.Description.ToLower().Contains(filter.Description));
        }
    }
}