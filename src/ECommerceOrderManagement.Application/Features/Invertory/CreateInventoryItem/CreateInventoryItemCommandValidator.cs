using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;

public sealed class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty()
            .WithErrorCode(InventoryValidationErrors.ProductIdRequiredCode)
            .WithMessage(InventoryValidationErrors.ProductIdRequiredMessage);

        RuleFor(command => command.InitialQuantity)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(InventoryValidationErrors.InitialQuantityInvalidCode)
            .WithMessage(InventoryValidationErrors.InitialQuantityInvalidMessage);

        RuleFor(command => command.ReorderLevel)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(InventoryValidationErrors.ReorderLevelInvalidCode)
            .WithMessage(InventoryValidationErrors.ReorderLevelInvalidMessage);
    }
}