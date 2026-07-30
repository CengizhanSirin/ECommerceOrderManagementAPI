using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Invertory.DecreaseStock;

public sealed class DecreaseStockCommandValidator : AbstractValidator<DecreaseStockCommand>
{
    public DecreaseStockCommandValidator()
    {
        RuleFor(command => command.ProductId)
        .NotEmpty()
        .WithErrorCode(InventoryValidationErrors.ProductIdRequiredCode)
        .WithMessage(InventoryValidationErrors.ProductIdRequiredMessage);

        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithErrorCode(InventoryValidationErrors.QuantityMustBePositiveCode)
            .WithMessage(InventoryValidationErrors.QuantityMustBePositiveMessage);
    }
}