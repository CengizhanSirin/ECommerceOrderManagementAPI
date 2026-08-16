using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid ShippingAddressId, Guid BillingAddressId, IReadOnlyCollection<CreateOrderItem> Items) : ICommand<CreateOrderResponse>;