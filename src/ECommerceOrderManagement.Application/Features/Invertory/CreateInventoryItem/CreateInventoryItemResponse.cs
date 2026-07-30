namespace ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;

public sealed record CreateInventoryItemResponse
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }
}