using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Specification
{
    public class ProductIdsSpec : Specification<Product>
    {
        public ProductIdsSpec(IEnumerable<Guid> ids)
        {
            Query.Where(x => ids.Contains(x.Id));
        }
    }
}