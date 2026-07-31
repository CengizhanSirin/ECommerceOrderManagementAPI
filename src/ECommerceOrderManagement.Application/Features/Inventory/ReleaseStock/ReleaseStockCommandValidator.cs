using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Inventory.ReleaseStock;

public sealed class ReleaseStockCommandValidator : AbstractValidator<ReleaseStockCommand>
{
    public ReleaseStockCommandValidator()
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