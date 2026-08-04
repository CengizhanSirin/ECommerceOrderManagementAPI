using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

namespace ECommerceOrderManagement.Application.Features.Orders;

public interface IOrderQueries
{
    Task<GetOrderByIdResponse?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}