using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.ProductId)
       .NotEmpty()
       .WithErrorCode(ProductValidationErrors.IdRequiredCode)
       .WithMessage(ProductValidationErrors.IdRequiredMessage);
    }
}