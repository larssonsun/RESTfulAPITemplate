using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Entity;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly RESTfulAPISampleContext _context;

        public ProductRepository(RESTfulAPISampleContext context)
        {
            _context = context;

            if (_context.Products.Count() == 0)
            {
                var now = DateTime.Now;
                _context.Products.AddRange(
                    new Product
                    {
                        Name = "Learning ASP.NET Core",
                        Description = "A best-selling book covering the fundamentals of ASP.NET Core",
                        IsOnSale = true,
                        CreateTime = now,
                    },
                    new Product
                    {
                        Name = "Learning EF Core",
                        Description = "A best-selling book covering the fundamentals of Entity Framework Core",
                        IsOnSale = true,
                        CreateTime = now,
                    },
                    new Product
                    {
                        Name = "Learning .NET Standard",
                        Description = "A best-selling book covering the fundamentals of .NET Standard",
                        CreateTime = now,
                    },
                    new Product
                    {
                        Name = "Learning .NET Core",
                        Description = "A best-selling book covering the fundamentals of .NET Core",
                        CreateTime = now,
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

        public async Task<PaginatedList<Product>> GetProducts(ProductDTOParameters parm)
        {
            var query = _context.Products.OrderBy(x => x.Id);
            var count = await query.CountAsync();
            var data = await query
                .Skip(parm.PageSize * parm.PageIndex)
                .Take(parm.PageSize).ToListAsync();

            return new PaginatedList<Product>(parm.PageIndex, parm.PageSize, count, data);
        }        
    }
}