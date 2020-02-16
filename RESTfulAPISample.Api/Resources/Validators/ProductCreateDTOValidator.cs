using FluentValidation;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Api.Resource.Validator
{
    public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateDTOValidator(IProductRepository productRepository)
        {
            RuleFor(p => p.Name).NotEmpty().WithName("产品名称").WithMessage("“{PropertyName}”是必填项")
                .MinimumLength(2).WithMessage("“{PropertyName}”的长度不可少于{MinLength}位")
                .MustAsync(async (pName, token) => await productRepository.CountNameWithString(pName) < 3).When(p => p.Name.Trim().Length > 6)
                    .WithMessage("“{PropertyName}”包含“{PropertyValue}”的记录至多只能有3条");
        }

    }
}