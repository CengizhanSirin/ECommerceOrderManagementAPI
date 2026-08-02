using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Exceptions;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Inventory;

internal static class InventoryUnitOfWorkExtensions
{
    public static async Task<Result> SaveInventoryChangesAsync(this IUnitOfWork unitOfWork, Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyConflictException)
        {
            return Result.Failure(InventoryErrors.ConcurrencyConflict(productId));
        }
    }
}