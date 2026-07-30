using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Invertory.DecreaseStock;

public sealed record DecreaseStockCommand(Guid ProductId, int Quantity) : ICommand
{
}