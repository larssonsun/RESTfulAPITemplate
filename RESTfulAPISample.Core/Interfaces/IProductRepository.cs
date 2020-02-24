using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfulAPISample.Core.DomainModel;
using RESTfulAPISample.Core.Entity;

namespace RESTfulAPISample.Core.Interface
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Task<PagedListBase<Product>> GetProducts(ProductDTOParameters parm);
        IAsyncEnumerable<Product> GetProductsEachAsync();
        Task<int> CountNameWithString(string s);
        Task<(bool hasProduct, Product product)> TryGetProduct(Guid id);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);

    }
}