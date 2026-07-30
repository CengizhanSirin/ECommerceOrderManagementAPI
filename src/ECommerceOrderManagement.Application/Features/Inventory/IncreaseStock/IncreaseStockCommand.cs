using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.IncreaseStock;

public sealed record IncreaseStockCommand(Guid ProductId, int Quantity, string? Reason) : ICommand
{
}