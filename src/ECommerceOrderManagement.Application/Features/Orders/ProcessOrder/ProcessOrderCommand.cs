using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.ProcessOrder;

public sealed record ProcessOrderCommand(Guid OrderId): ICommand;