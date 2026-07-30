using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;

namespace ECommerceOrderManagement.Application.Features.Invertory;

public interface IInventoryQueries
{
    Task<GetInventoryByProductIdResponse?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}