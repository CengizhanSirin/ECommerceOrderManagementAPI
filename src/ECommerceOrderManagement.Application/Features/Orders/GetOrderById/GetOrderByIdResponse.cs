using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed record GetOrderByIdResponse(Guid Id, Guid CustomerId, string OrderNumber, OrderStatus Status, decimal Subtotal, decimal TotalAmount,
    string? CancellationReason,
    GetOrderByIdAddressResponse ShippingAddress,
    GetOrderByIdAddressResponse BillingAddress,
    IReadOnlyCollection<GetOrderByIdItemResponse> Items,
    DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);