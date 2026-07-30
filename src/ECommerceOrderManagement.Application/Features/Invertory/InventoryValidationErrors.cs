namespace ECommerceOrderManagement.Application.Features.Invertory;

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
}