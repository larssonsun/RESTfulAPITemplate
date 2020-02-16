using FluentValidation;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Api.Resource.Validator
{
    public class ProductUpdateDTOValidator : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator(IProductRepository productRepository)
        {
            RuleFor(p => p.Name)
                .NotNull().WithMessage("属性：“{PropertyName}”是必须的")
                .NotEmpty().WithMessage("“{PropertyName}”的长度不能为0")
                .MinimumLength(2).WithMessage("“{PropertyName}”的长度不可小{MinLength}位")
                .MustAsync(async (pName, token) => pName.Trim().Length < 6 ? true :
                    await productRepository.CountNameWithString(pName) < 3)
                    .WithMessage("“{PropertyName}”包含“{PropertyValue}”的记录至多只能有3条")
                .WithName("产品名称");
        }
    }
}