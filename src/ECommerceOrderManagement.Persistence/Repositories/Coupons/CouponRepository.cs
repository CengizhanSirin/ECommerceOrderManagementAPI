using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Coupons;

internal sealed class CouponRepository(ApplicationDbContext dbContext) : Repository<Coupon>(dbContext), ICouponRepository
{
    // Aynı kuponun eş zamanlı siparişlerde kullanım limitini aşmasını önlemek için
    // ilgili kupon satırında transaction sonuna kadar update lock tutulur.
    // Böylece aynı kuponu kullanan paralel istekler sıraya girer ve kullanım sayısı
    // her istek için güncel haliyle tekrar okunur.
    // UPDLOCK + HOLDLOCK, aynı kupon üzerinde yarışan işlemlerin deadlock yerine kontrollü şekilde beklemesini hedefler.
    public async Task AcquireUsageLockAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        await dbContext.Coupons
            .FromSqlInterpolated($"""
            SELECT *FROM [discount].[Coupons] WITH (UPDLOCK, HOLDLOCK)WHERE [Code] = {normalizedCode} 
            """)
            .AsNoTracking()
            .Select(coupon => coupon.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }

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