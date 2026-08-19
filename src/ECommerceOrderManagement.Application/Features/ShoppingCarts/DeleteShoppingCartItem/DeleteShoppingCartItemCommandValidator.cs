using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.DeleteShoppingCartItem;

public sealed class DeleteShoppingCartItemCommandValidator : AbstractValidator<DeleteShoppingCartItemCommand>
{
    public DeleteShoppingCartItemCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithErrorCode(ShoppingCartValidationErrors.ProductIdRequiredCode)
            .WithMessage(ShoppingCartValidationErrors.ProductIdRequiredMessage);
    }
}