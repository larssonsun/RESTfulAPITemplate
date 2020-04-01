using System;
using System.Collections.Generic;
using Larsson.RESTfulAPIHelper.SortAndQuery;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Entity;

namespace RESTfulAPITemplate.Core.Configuration.SortMapping
{
    public class ProductSortMapping : PropertyMapping<ProductDTO, Product>
    {
        public ProductSortMapping() :
            base(new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(ProductDTO.FullName)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.Name), Revert = false},
                    new MappedProperty{ Name = nameof(Product.Description), Revert = false}
                },
                [nameof(ProductDTO.Name)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.Name), Revert = false}
                },
                [nameof(ProductDTO.Description)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.Description), Revert = false}
                },
                [nameof(ProductDTO.CreateTime)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.CreateTime), Revert = false}
                }
            })
        {
        }
    }
}