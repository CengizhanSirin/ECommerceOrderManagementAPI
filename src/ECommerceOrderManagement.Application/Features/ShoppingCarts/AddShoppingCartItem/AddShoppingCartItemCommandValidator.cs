using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;

public sealed class AddShoppingCartItemCommandValidator : AbstractValidator<AddShoppingCartItemCommand>
{
    public AddShoppingCartItemCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithErrorCode(ShoppingCartValidationErrors.ProductIdRequiredCode)
            .WithMessage(ShoppingCartValidationErrors.ProductIdRequiredMessage);

        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithErrorCode(ShoppingCartValidationErrors.QuantityGreaterThanZeroCode)
            .WithMessage(ShoppingCartValidationErrors.QuantityGreaterThanZeroMessage);
    }
}