using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.Inventory.IncreaseStock;
using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Invertory.IncreaseStock;

public sealed class IncreaseStockCommandValidator : AbstractValidator<IncreaseStockCommand>
{
    public IncreaseStockCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithErrorCode(InventoryValidationErrors.ProductIdRequiredCode)
            .WithMessage(InventoryValidationErrors.ProductIdRequiredMessage);

        RuleFor(command => command.Quantity)
            .GreaterThan(0)
            .WithErrorCode(InventoryValidationErrors.QuantityMustBePositiveCode)
            .WithMessage(InventoryValidationErrors.QuantityMustBePositiveMessage);

        RuleFor(command => command.Reason)
            .MaximumLength(500)
            .WithErrorCode(InventoryValidationErrors.ReasonTooLongCode)
            .WithMessage(InventoryValidationErrors.ReasonTooLongMessage);
    }
}