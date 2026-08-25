using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons;

public interface ICouponRepository : IRepository<Coupon>
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task AddUsageAsync(CouponUsage usage, CancellationToken cancellationToken = default);
}