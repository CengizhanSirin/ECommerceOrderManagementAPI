using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid ShippingAddressId, CreateOrderAddress BillingAddress, IReadOnlyCollection<CreateOrderItem> Items) : ICommand<CreateOrderResponse>;