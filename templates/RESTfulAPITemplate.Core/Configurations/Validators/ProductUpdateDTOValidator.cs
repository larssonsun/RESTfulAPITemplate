using FluentValidation;
using RESTfulAPITemplate.Core.DTO;
using RESTfulAPITemplate.Core.Interface;

namespace RESTfulAPITemplate.Core.Validator
{
    public class ProductUpdateDTOValidator : ProductCreateOrUpdateDTOValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}