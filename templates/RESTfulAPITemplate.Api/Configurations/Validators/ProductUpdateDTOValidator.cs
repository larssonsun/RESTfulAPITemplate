using FluentValidation;
using RESTfulAPITemplate.App.Model;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.App.Validator
{
    public class ProductUpdateDTOValidator : ProductCreateOrUpdateDTOValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}