using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.ShipOrder;

public sealed record ShipOrderCommand(Guid OrderId) : ICommand;