using ECommerceOrderManagement.Domain.Inventory;
using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;

public sealed class GetStockMovementHistoryQueryValidator : AbstractValidator<GetStockMovementHistoryQuery>
{

    private static readonly string[] AllowedSortDirections =
       [
           "asc",
        "desc"
       ];

    public GetStockMovementHistoryQueryValidator()
    {
        RuleFor(query => query.ProductId)
            .NotEmpty()
            .WithErrorCode(InventoryValidationErrors.ProductIdRequiredCode)
            .WithMessage(InventoryValidationErrors.ProductIdRequiredMessage);

        RuleFor(query => query.Page)
            .GreaterThan(0)
            .WithErrorCode(InventoryValidationErrors.PageInvalidCode)
            .WithMessage(InventoryValidationErrors.PageInvalidMessage);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode(InventoryValidationErrors.PageSizeInvalidCode)
            .WithMessage(InventoryValidationErrors.PageSizeInvalidMessage);

        RuleFor(query => query.Type)
            .Must(BeValidMovementType)
            .WithErrorCode(InventoryValidationErrors.StockMovementTypeInvalidCode)
            .WithMessage(InventoryValidationErrors.StockMovementTypeInvalidMessage);

        RuleFor(query => query.SortDirection)
            .NotEmpty()
            .Must(BeSupportedSortDirection)
            .WithErrorCode(InventoryValidationErrors.SortDirectionInvalidCode)
            .WithMessage(InventoryValidationErrors.SortDirectionInvalidMessage);
    }

    private static bool BeValidMovementType(StockMovementType? type)
    {
        return type is null || Enum.IsDefined(type.Value);
    }

    private static bool BeSupportedSortDirection(string sortDirection)
    {
        return AllowedSortDirections.Contains(sortDirection, StringComparer.OrdinalIgnoreCase);
    }
}