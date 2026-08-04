using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrders;

internal sealed class GetOrdersQueryHandler(IOrderQueries orderQueries) : IQueryHandler<GetOrdersQuery, PagedResult<GetOrdersItemResponse>>
{
    public async Task<Result<PagedResult<GetOrdersItemResponse>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pagedOrders = await orderQueries.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<GetOrdersItemResponse>>.Success(pagedOrders);
    }
}