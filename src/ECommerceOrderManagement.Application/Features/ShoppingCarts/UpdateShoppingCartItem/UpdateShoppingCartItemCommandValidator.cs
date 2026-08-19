using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.ShoppingCarts.UpdateShoppingCartItem;

public sealed class UpdateShoppingCartItemCommandValidator : AbstractValidator<UpdateShoppingCartItemCommand>
{
    public UpdateShoppingCartItemCommandValidator()
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