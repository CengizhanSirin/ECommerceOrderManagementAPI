using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Invertory;

internal static class InventoryErrors
{
    public static Error NotFound(Guid productId)
    {
        return Error.NotFound("Inventory.NotFound", $"An inventory item for product ID '{productId}' was not found.");
    }

    public static Error ProductNotFound(Guid productId)
    {
        return Error.NotFound("Inventory.ProductNotFound", $"The product with ID '{productId}' was not found.");
    }

    public static Error AlreadyExists(Guid productId)
    {
        return Error.Conflict("Inventory.AlreadyExists", $"An inventory item for product ID '{productId}' already exists.");
    }

    public static Error InsufficientStock(int requestedQuantity, int availableQuantity)
    {
        return Error.Conflict("Inventory.InsufficientStock", $"The requested quantity '{requestedQuantity}' exceeds the available stock '{availableQuantity}'.");
    }

    public static Error ConcurrencyConflict(Guid productId)
    {
        return Error.Conflict("Inventory.ConcurrencyConflict", $"The inventory item for product ID '{productId}' was modified by another operation. Please retry.");
    }
}