using FluentValidation;
using RESTfulAPISample.Core.DTO;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Core.Validator
{
    public class ProductUpdateDTOValidator : ProductCreateOrUpdateDTOValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository) : base(productRepository)
        {
        }
    }
}