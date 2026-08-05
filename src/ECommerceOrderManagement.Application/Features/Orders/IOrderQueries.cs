using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;
using ECommerceOrderManagement.Application.Features.Orders.GetOrders;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders;

public interface IOrderQueries
{
    Task<GetOrderByIdResponse?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<PagedResult<GetOrdersItemResponse>> GetPagedAsync(GetOrdersQuery query, CancellationToken cancellationToken = default);
}