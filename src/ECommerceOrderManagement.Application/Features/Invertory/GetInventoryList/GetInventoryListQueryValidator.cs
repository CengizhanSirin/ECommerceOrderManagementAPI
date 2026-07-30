using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Invertory.GetInventoryList;

public sealed class GetInventoryListQueryValidator : AbstractValidator<GetInventoryListQuery>
{
    private static readonly string[] AllowedSortFields =
        [
          "productName",
          "sku",
          "quantityOnHand",
          "availableQuantity",
          "reorderLevel",
          "createdAtUtc"
        ];

    private static readonly string[] AllowedSortDirections = ["asc", "desc"];


    public GetInventoryListQueryValidator()
    {
        RuleFor(query => query.Page)
          .GreaterThan(0)
          .WithErrorCode(InventoryValidationErrors.PageInvalidCode)
          .WithMessage(InventoryValidationErrors.PageInvalidMessage);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode(InventoryValidationErrors.PageSizeInvalidCode)
            .WithMessage(InventoryValidationErrors.PageSizeInvalidMessage);

        RuleFor(query => query.Search)
            .MaximumLength(100)
            .WithErrorCode(InventoryValidationErrors.SearchTooLongCode)
            .WithMessage(InventoryValidationErrors.SearchTooLongMessage);

        RuleFor(query => query.SortBy)
            .NotEmpty()
            .Must(BeSupportedSortField)
            .WithErrorCode(InventoryValidationErrors.SortByInvalidCode)
            .WithMessage(InventoryValidationErrors.SortByInvalidMessage);

        RuleFor(query => query.SortDirection)
            .NotEmpty()
            .Must(BeSupportedSortDirection)
            .WithErrorCode(InventoryValidationErrors.SortDirectionInvalidCode)
            .WithMessage(InventoryValidationErrors.SortDirectionInvalidMessage);
    }

    private static bool BeSupportedSortField(string sortBy) => AllowedSortFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase);

    private static bool BeSupportedSortDirection(string sortDirection) => AllowedSortDirections.Contains(sortDirection, StringComparer.OrdinalIgnoreCase);

}