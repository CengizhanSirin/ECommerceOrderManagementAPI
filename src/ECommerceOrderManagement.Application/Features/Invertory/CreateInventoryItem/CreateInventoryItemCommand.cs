using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;

public sealed record CreateInventoryItemCommand(Guid ProductId, int InitialQuantity, int ReorderLevel) : ICommand<CreateInventoryItemResponse>
{
}