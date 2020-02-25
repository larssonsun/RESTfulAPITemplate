using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if (RESTFULAPIHELPER)
using Larsson.RESTfulAPIHelper.Interface;
using Larsson.RESTfulAPIHelper.SortAndQuery;
#endif
using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.DTO;
using RESTfulAPISample.Core.Entity;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly RESTfulAPISampleContext _context;

#if (RESTFULAPIHELPER)

        private readonly IPropertyMappingContainer _propertyMappingContainer;

#endif

#if (RESTFULAPIHELPER)

        public ProductRepository(RESTfulAPISampleContext context, IPropertyMappingContainer propertyMappingContainer)
        {
            _context = context;
            _propertyMappingContainer = propertyMappingContainer;
#else

        public ProductRepository(RESTfulAPISampleContext context)
        {
            _context = context;

#endif


            if (_context.Products.Count() == 0)
            {
                var now = DateTime.Now;
                _context.Products.AddRange(
                    new Product
                    {
                        Name = "A Learning ASP.NET Core",
                        Description = "C best-selling book covering the fundamentals of ASP.NET Core",
                        IsOnSale = true,
                        CreateTime = now.AddDays(1),
                    },
                    new Product
                    {
                        Name = "D Learning EF Core",
                        Description = "A best-selling book covering the fundamentals of C#",
                        IsOnSale = true,
                        CreateTime = now,
                    },
                    new Product
                    {
                        Name = "D Learning EF Core",
                        Description = "B best-selling book covering the fundamentals of .NET Standard",
                        CreateTime = now.AddDays(2),
                    },
                    new Product
                    {
                        Name = "C Learning .NET Core",
                        Description = "D best-selling book covering the fundamentals of .NET Core",
                        CreateTime = now.AddDays(13),
                    },
                    new Product
                    {
                        Name = "Learning C#",
                        Description = "A best-selling book covering the fundamentals of C#",
                        CreateTime = now,
                    });
                _context.SaveChanges();
            }
        }

        public IAsyncEnumerable<Product> GetProductsEachAsync() =>
            _context.Products.OrderBy(p => p.Name).AsNoTracking().AsAsyncEnumerable();

        public async Task<int> CountNameWithString(string s)
        {
            return await _context.Products.CountAsync(x => x.Name.Contains(s));
        }

        public async Task<(bool hasProduct, Product product)> TryGetProduct(Guid id)
        {
            var result = await _context.Products.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

            return (result != null, result);
        }

        public void AddProduct(Product product)
        {
            product.CreateTime = DateTime.Now;
            _context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Update(product);
        }

        public async

#if (RESTFULAPIHELPER)

        Task<PagedListBase<Product>>

#else

        Task<IEnumerable<Product>> 

#endif
        GetProducts(ProductDTOParameters parameters)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                var name = parameters.Name.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLowerInvariant() == name);
            }

            if (!string.IsNullOrEmpty(parameters.Description))
            {
                var description = parameters.Description.Trim().ToLowerInvariant();
                query = query.Where(x => x.Description.ToLowerInvariant().Contains(description));
            }

#if (RESTFULAPIHELPER)

            query = query.ApplySort(parameters.OrderBy, _propertyMappingContainer.Resolve<ProductDTO, Product>());

#endif

            var count = await query.CountAsync();
            var data = await query

#if (RESTFULAPIHELPER)

                .Skip(parameters.PageSize * parameters.PageIndex)
                .Take(parameters.PageSize)

#endif

                .AsNoTracking().ToListAsync();

#if (RESTFULAPIHELPER)

            return new PagedListBase<Product>(parameters.PageIndex, parameters.PageSize, count, data);

#else

            return data;

#endif
        }
    }
}