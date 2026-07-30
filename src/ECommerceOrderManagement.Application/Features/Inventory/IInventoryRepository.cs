using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.Application.Features.Inventory;

public interface IInventoryRepository : IRepository<InventoryItem>
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}