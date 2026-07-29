using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;

public sealed class GetBrandByIdQueryValidator : AbstractValidator<GetBrandByIdQuery>
{
    public GetBrandByIdQueryValidator()
    {
        RuleFor(query => query.BrandId)
            .NotEmpty()
            .WithErrorCode(BrandValidationErrors.IdRequiredCode)
            .WithMessage(BrandValidationErrors.IdRequiredMessage);
    }
}