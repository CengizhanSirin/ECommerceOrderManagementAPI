using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.ReserveStock;

public sealed record ReserveStockCommand(Guid ProductId, int Quantity, string? Reason) : ICommand
{
}