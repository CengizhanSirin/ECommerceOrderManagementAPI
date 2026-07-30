using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Invertory.IncreaseStock;

public sealed record IncreaseStockCommand(Guid ProductId, int Quantity) : ICommand
{
}