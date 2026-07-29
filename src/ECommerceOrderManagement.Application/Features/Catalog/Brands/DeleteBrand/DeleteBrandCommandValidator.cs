using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.DeleteBrand;

public sealed class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(command => command.BrandId)
            .NotEmpty()
            .WithErrorCode(BrandValidationErrors.IdRequiredCode)
            .WithMessage(BrandValidationErrors.IdRequiredMessage);
    }
}