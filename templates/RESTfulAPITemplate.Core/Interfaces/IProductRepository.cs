using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfulAPITemplate.Core.DomainModel;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Interface
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
#if (!OBSOLETESQLSERVER)

        IAsyncEnumerable<Product> GetProductsEachAsync();

#endif

        Task<int> CountNameWithString(string s);
    }
}