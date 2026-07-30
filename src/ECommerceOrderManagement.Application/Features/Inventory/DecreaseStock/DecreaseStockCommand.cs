using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.DecreaseStock;

public sealed record DecreaseStockCommand(Guid ProductId, int Quantity, string? Reason) : ICommand
{
}