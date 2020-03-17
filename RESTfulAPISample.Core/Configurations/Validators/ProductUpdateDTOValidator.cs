using FluentValidation;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Core.DTO.Validator
{
    public class ProductUpdateDTOValidator : ProductCreateOrUpdateDTOValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}