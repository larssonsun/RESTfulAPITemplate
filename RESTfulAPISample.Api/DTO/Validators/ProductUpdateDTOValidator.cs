using FluentValidation;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Api.DTO.Validator
{
    public class ProductUpdateDTOValidator : ProductCreateOrUpdateDTOValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}