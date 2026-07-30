namespace ECommerceOrderManagement.API.Features.Inventory.CreateInvertoryItem;

public sealed class CreateInventoryItemRequest
{
    public int InitialQuantity { get; init; }

    public int ReorderLevel { get; init; }
}