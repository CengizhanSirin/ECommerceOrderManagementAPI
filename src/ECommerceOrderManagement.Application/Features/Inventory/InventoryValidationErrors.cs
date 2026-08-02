namespace ECommerceOrderManagement.Application.Features.Inventory;

internal static class InventoryValidationErrors
{
    public const string ProductIdRequiredCode = "Inventory.ProductId.Required";
    public const string ProductIdRequiredMessage = "Product ID is required.";
    public const string InitialQuantityInvalidCode = "Inventory.InitialQuantity.Invalid";
    public const string InitialQuantityInvalidMessage = "Initial quantity cannot be negative.";
    public const string ReorderLevelInvalidCode = "Inventory.ReorderLevel.Invalid";
    public const string ReorderLevelInvalidMessage = "Reorder level cannot be negative.";
    public const string QuantityMustBePositiveCode = "Inventory.Quantity.MustBePositive";
    public const string QuantityMustBePositiveMessage = "Quantity must be greater than zero.";
    public const string PageInvalidCode = "Inventory.Page.Invalid";
    public const string PageInvalidMessage = "Page must be greater than zero.";
    public const string PageSizeInvalidCode = "Inventory.PageSize.Invalid";
    public const string PageSizeInvalidMessage = "Page size must be between 1 and 100.";
    public const string SearchTooLongCode = "Inventory.Search.TooLong";
    public const string SearchTooLongMessage = "Search cannot exceed 100 characters.";
    public const string SortByInvalidCode = "Inventory.SortBy.Invalid";
    public const string SortByInvalidMessage = "Sort field is not supported.";
    public const string SortDirectionInvalidCode = "Inventory.SortDirection.Invalid";
    public const string SortDirectionInvalidMessage = "Sort direction must be either 'asc' or 'desc'.";
    public const string ReasonTooLongCode = "Inventory.Reason.TooLong";
    public const string ReasonTooLongMessage = "Reason cannot exceed 500 characters.";
    public const string StockMovementTypeInvalidCode = "Inventory.StockMovementType.Invalid";
    public const string StockMovementTypeInvalidMessage = "Stock movement type is invalid.";
}