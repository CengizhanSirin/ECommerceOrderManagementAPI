namespace ECommerceOrderManagement.Application.Features.Coupons;

public interface ICouponQueries
{
    Task<CouponEvaluationReadModel?> GetForEvaluationAsync(string code, Guid userId, CancellationToken cancellationToken = default);
}