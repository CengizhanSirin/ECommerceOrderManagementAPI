using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;

public sealed record GetInventoryByProductIdQuery(Guid ProductId) : IQuery<GetInventoryByProductIdResponse>
{
}