using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;
using ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;
using ECommerceOrderManagement.Domain.Coupons;
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

    public async Task<PagedResult<GetCouponsItemResponse>> GetPagedAsync(GetCouponsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Coupon> couponsQuery = dbContext.Coupons.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var normalizedSearchTerm = query.SearchTerm.Trim().ToUpperInvariant();

            couponsQuery = couponsQuery.Where(coupon => coupon.Code.Contains(normalizedSearchTerm));
        }

        if (query.DiscountType.HasValue)
        {
            couponsQuery = couponsQuery.Where(coupon => coupon.DiscountType == query.DiscountType.Value);
        }

        if (query.IsActive.HasValue)
        {
            couponsQuery = couponsQuery.Where(coupon => coupon.IsActive == query.IsActive.Value);
        }

        var totalCount = await couponsQuery.CountAsync(cancellationToken);

        var orderedQuery = ApplySorting(couponsQuery, query.SortBy, query.SortDirection);

        var items = await orderedQuery
            .ThenBy(coupon => coupon.Id)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(coupon => new GetCouponsItemResponse(
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
                coupon.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetCouponsItemResponse>(items, query.PageNumber, query.PageSize, totalCount);
    }

    private static IOrderedQueryable<Coupon> ApplySorting(IQueryable<Coupon> query, string? sortBy, string? sortDirection)
    {
        var normalizedSortBy = string.IsNullOrWhiteSpace(sortBy)
            ? "createdatutc"
            : sortBy.Trim().ToLowerInvariant();

        var isDescending =
            string.IsNullOrWhiteSpace(sortDirection)
            ||
            sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return (normalizedSortBy, isDescending) switch
        {
            ("code", false) => query.OrderBy(coupon => coupon.Code),

            ("code", true) => query.OrderByDescending(coupon => coupon.Code),

            ("discountvalue", false) => query.OrderBy(coupon => coupon.DiscountValue),

            ("discountvalue", true) => query.OrderByDescending(coupon => coupon.DiscountValue),

            ("minimumorderamount", false) => query.OrderBy(coupon => coupon.MinimumOrderAmount),

            ("minimumorderamount", true) => query.OrderByDescending(coupon => coupon.MinimumOrderAmount),

            ("startsatutc", false) => query.OrderBy(coupon => coupon.StartsAtUtc),

            ("startsatutc", true) => query.OrderByDescending(coupon => coupon.StartsAtUtc),

            ("endsatutc", false) => query.OrderBy(coupon => coupon.EndsAtUtc),

            ("endsatutc", true) => query.OrderByDescending(coupon => coupon.EndsAtUtc),

            ("usagelimit", false) => query.OrderBy(coupon => coupon.UsageLimit),

            ("usagelimit", true) => query.OrderByDescending(coupon => coupon.UsageLimit),

            ("createdatutc", false) => query.OrderBy(coupon => coupon.CreatedAtUtc),

            _ =>
                query.OrderByDescending(coupon => coupon.CreatedAtUtc)
        };
    }

}