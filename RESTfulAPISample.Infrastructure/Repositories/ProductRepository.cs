using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.DTO;
using RESTfulAPISample.Core.Entity;
using RESTfulAPISample.Core.Interface;
using RESTfulAPISample.Core.Pagination;
using RESTfulAPISample.Core.SortAndQuery;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly RESTfulAPISampleContext _context;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public ProductRepository(RESTfulAPISampleContext context, IPropertyMappingContainer propertyMappingContainer)
        {
            _context = context;
            _propertyMappingContainer = propertyMappingContainer;

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
            _context.Products.OrderBy(p => p.Name).AsAsyncEnumerable();

        public async Task<int> CountNameWithString(string s)
        {
            return await _context.Products.CountAsync(x => x.Name.Contains(s));
        }

        public async Task<(bool hasProduct, Product product)> TryGetProduct(Guid id)
        {
            var result = await _context.Products.FindAsync(id);

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

        public async Task<PaginatedList<Product>> GetProducts(ProductDTOParameters parameters)
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

            query = query.ApplySort(parameters.OrderBy, _propertyMappingContainer.Resolve<ProductDTO, Product>());

            var count = await query.CountAsync();
            var data = await query
                .Skip(parameters.PageSize * parameters.PageIndex)
                .Take(parameters.PageSize).ToListAsync();

            return new PaginatedList<Product>(parameters.PageIndex, parameters.PageSize, count, data);
        }
    }
}