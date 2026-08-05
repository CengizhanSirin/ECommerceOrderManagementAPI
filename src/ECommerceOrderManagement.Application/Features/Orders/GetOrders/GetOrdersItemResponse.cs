using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrders;

public sealed record GetOrdersItemResponse(Guid Id, Guid CustomerId, string OrderNumber, OrderStatus Status, int ItemCount, decimal TotalAmount, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);