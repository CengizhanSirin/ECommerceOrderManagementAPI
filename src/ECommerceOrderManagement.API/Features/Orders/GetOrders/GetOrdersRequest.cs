using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.API.Features.Orders.GetOrders;

public sealed class GetOrdersRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }

    public OrderStatus? Status { get; init; }

    public string? SortBy { get; init; } = "createdAtUtc";

    public string? SortDirection { get; init; } = "desc";
}