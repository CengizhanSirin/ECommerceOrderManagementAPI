using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Orders;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId)
    {
        return Error.NotFound("Orders.NotFound", $"The order with id '{orderId}' was not found.");
    }

    public static Error ProductNotFound(Guid productId)
    {
        return Error.NotFound("Orders.ProductNotFound", $"The product with id '{productId}' was not found.");
    }

    public static Error InventoryNotFound(Guid productId)
    {
        return Error.NotFound("Orders.InventoryNotFound", $"Inventory was not found for product '{productId}'.");
    }

    public static Error InsufficientStock(Guid productId)
    {
        return Error.Conflict("Orders.InsufficientStock", $"There is insufficient stock for product '{productId}'.");
    }

    public static Error OrderNumberConflict(string orderNumber)
    {
        return Error.Conflict("Orders.OrderNumberConflict", $"The order number '{orderNumber}' is already in use.");
    }

    public static Error CannotCancel(Guid orderId)
    {
        return Error.Conflict("Orders.CannotCancel", $"The order with id '{orderId}' cannot be cancelled in its current status.");
    }

    public static Error CannotStartProcessing(Guid orderId)
    {
        return Error.Conflict("Orders.CannotStartProcessing", $"The order with id '{orderId}' cannot start processing in its current status.");
    }

    public static Error CannotShip(Guid orderId)
    {
        return Error.Conflict("Orders.CannotShip", $"The order with id '{orderId}' cannot be shipped in its current status.");
    }

    public static Error CannotDeliver(Guid orderId)
    {
        return Error.Conflict("Orders.CannotDeliver", $"The order with id '{orderId}' cannot be delivered in its current status.");
    }

    public static Error ReservationConflict(Guid productId)
    {
        return Error.Conflict("Orders.ReservationConflict", $"The reserved stock is insufficient for product '{productId}'.");
    }
}