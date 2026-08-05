using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId, CreateOrderAddress ShippingAddress,CreateOrderAddress BillingAddress,IReadOnlyCollection<CreateOrderItem> Items): ICommand<CreateOrderResponse>;
