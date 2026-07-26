using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;

public sealed class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(query => query.ProductId)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.IdRequiredCode)
            .WithMessage(ProductValidationErrors.IdRequiredMessage);
    }
}