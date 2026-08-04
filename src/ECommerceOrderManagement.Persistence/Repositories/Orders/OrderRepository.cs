using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Domain.Orders;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Orders;

internal sealed class OrderRepository(ApplicationDbContext dbContext) : Repository<Order>(dbContext), IOrderRepository
{
    public Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return DbSet.Include(order => order.Items).SingleOrDefaultAsync(order => order.Id == orderId, cancellationToken);
    }

    public Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(order => order.OrderNumber == orderNumber, cancellationToken);
    }
}