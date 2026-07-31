using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.ReleaseStock;

public sealed record ReleaseStockCommand(Guid ProductId, int Quantity, string? Reason) : ICommand
{
}