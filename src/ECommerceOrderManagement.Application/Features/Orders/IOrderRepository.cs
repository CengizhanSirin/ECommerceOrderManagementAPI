using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAndCustomerIdWithItemsAsync(Guid orderId, Guid customerId, CancellationToken cancellationToken = default);
}