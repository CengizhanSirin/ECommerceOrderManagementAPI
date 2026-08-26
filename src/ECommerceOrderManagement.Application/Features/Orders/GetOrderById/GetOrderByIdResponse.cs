using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed record GetOrderByIdResponse(Guid Id, Guid CustomerId,
    string OrderNumber,
    OrderStatus Status,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TotalAmount,
    string? CouponCode,
    DiscountType? DiscountType,
    decimal? DiscountValue,
    string? CancellationReason,
    GetOrderByIdAddressResponse ShippingAddress,
    GetOrderByIdAddressResponse BillingAddress,
    IReadOnlyCollection<GetOrderByIdItemResponse> Items,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);