using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.DeliverOrder;

public sealed record DeliverOrderCommand( Guid OrderId): ICommand;