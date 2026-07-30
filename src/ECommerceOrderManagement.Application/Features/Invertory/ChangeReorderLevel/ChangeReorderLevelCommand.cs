using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Invertory.ChangeReorderLevel;

public sealed record ChangeReorderLevelCommand(Guid ProductId, int ReorderLevel) : ICommand
{
}