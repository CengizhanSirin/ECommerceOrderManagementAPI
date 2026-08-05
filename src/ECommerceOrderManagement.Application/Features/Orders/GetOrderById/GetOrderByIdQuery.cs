using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<GetOrderByIdResponse>;