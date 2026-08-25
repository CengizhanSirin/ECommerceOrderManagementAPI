using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Coupons.Queries;

internal sealed class CouponQueries(ApplicationDbContext dbContext) : ICouponQueries
{
    public Task<GetCouponByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Coupons
            .AsNoTracking()
            .Where(coupon => coupon.Id == id)
            .Select(coupon => new GetCouponByIdResponse(
                coupon.Id,
                coupon.Code,
                coupon.DiscountType,
                coupon.DiscountValue,
                coupon.MinimumOrderAmount,
                coupon.StartsAtUtc,
                coupon.EndsAtUtc,
                coupon.UsageLimit,
                coupon.UsageLimitPerUser,
                dbContext.CouponUsages.Count(usage => usage.CouponId == coupon.Id),
                coupon.IsActive,
                coupon.CreatedAtUtc,
                coupon.UpdatedAtUtc))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<CouponEvaluationReadModel?> GetForEvaluationAsync(string code, Guid userId, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return dbContext.Coupons
            .AsNoTracking()
            .Where(coupon => coupon.Code == normalizedCode)

            .Select(coupon => new CouponEvaluationReadModel(
                coupon.Id,
                coupon.Code,
                coupon.DiscountType,
                coupon.DiscountValue,
                coupon.MinimumOrderAmount,
                coupon.StartsAtUtc,
                coupon.EndsAtUtc,
                coupon.UsageLimit,
                coupon.UsageLimitPerUser,
                coupon.IsActive,
                dbContext.CouponUsages.Count(usage => usage.CouponId == coupon.Id),
                dbContext.CouponUsages.Count(usage =>
                usage.CouponId == coupon.Id && usage.UserId == userId)))

            .SingleOrDefaultAsync(cancellationToken);
    }

}