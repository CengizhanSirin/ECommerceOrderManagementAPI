using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;

public sealed record GetInventoryByProductIdQuery(Guid ProductId) : IQuery<GetInventoryByProductIdResponse>
{
}