using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId, string Reason) : ICommand;