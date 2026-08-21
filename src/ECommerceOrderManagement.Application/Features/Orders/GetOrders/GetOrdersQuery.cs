using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrders;

public sealed record GetOrdersQuery(int PageNumber, int PageSize, string? SearchTerm, OrderStatus? Status, string? SortBy, string? SortDirection)
    : IQuery<PagedResult<GetOrdersItemResponse>>;