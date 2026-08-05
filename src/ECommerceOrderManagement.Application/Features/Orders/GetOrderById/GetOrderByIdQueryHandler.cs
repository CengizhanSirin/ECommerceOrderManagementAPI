using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

internal sealed class GetOrderByIdQueryHandler(IOrderQueries orderQueries) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await orderQueries.GetByIdAsync(query.OrderId, cancellationToken);

        if (order is null)
        {
            return Result<GetOrderByIdResponse>.Failure(OrderErrors.NotFound(query.OrderId));
        }

        return Result<GetOrderByIdResponse>.Success(order);
    }
}