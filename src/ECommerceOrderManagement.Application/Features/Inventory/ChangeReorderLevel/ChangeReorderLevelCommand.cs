using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.ChangeReorderLevel;

public sealed record ChangeReorderLevelCommand(Guid ProductId, int ReorderLevel) : ICommand
{
}