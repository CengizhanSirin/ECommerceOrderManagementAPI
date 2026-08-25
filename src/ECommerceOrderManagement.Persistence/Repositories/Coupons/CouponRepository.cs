using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Coupons;

internal sealed class CouponRepository(ApplicationDbContext dbContext) : Repository<Coupon>(dbContext), ICouponRepository
{
    public async Task AddUsageAsync(CouponUsage usage, CancellationToken cancellationToken = default)
    {
        await dbContext.CouponUsages.AddAsync(usage, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await DbSet.AnyAsync(coupon => coupon.Code == normalizedCode, cancellationToken);
    }
}