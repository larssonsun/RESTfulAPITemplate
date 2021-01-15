using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Interface;
using Larsson.RESTfulAPIHelper.SortAndQuery;
#endif
using Microsoft.EntityFrameworkCore;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Entity;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Infrastructure.Repository
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        private readonly ProductContext _context;

#if (RESTFULAPIHELPER)

        private readonly IPropertyMappingContainer _propertyMappingContainer;

#endif

#if (RESTFULAPIHELPER)

        public ProductRepository(ProductContext context, IPropertyMappingContainer propertyMappingContainer) : base(context, propertyMappingContainer)
        {
            _context = context;
            _propertyMappingContainer = propertyMappingContainer;
#else

        public ProductRepository(ProductContext context) : base(context)
        {
            _context = context;

#endif

        }

#if (!OBSOLETESQLSERVER)

        public IAsyncEnumerable<Product> GetProductsEachAsync() =>
            _context.Products.OrderBy(p => p.Name).AsNoTracking().AsAsyncEnumerable();

#endif
        public async Task<int> CountNameWithString(string s)
        {
            return await _context.Products.CountAsync(x => x.Name.Contains(s));
        }
    }
}