using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Inventory.ChangeReorderLevel;

public sealed class ChangeReorderLevelCommandValidator : AbstractValidator<ChangeReorderLevelCommand>
{
    public ChangeReorderLevelCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithErrorCode(InventoryValidationErrors.ProductIdRequiredCode)
            .WithMessage(InventoryValidationErrors.ProductIdRequiredMessage);

        RuleFor(command => command.ReorderLevel)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(InventoryValidationErrors.ReorderLevelInvalidCode)
            .WithMessage(InventoryValidationErrors.ReorderLevelInvalidMessage);
    }
}