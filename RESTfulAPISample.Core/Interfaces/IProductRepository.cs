using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfulAPISample.Core.DomainModel;

namespace RESTfulAPISample.Core.Interface
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Task<IEnumerable<Product>> GetProducts();
        IAsyncEnumerable<Product> GetProductsAsync();
        Task<int> CountNameWithString(string s);
        Task<(bool hasProduct, Product product)> TryGetProduct(Guid id);
        void DeleteProduct(Product product);
    }
}