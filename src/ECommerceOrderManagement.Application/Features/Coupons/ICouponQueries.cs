using ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;

namespace ECommerceOrderManagement.Application.Features.Coupons;

public interface ICouponQueries
{
    Task<CouponEvaluationReadModel?> GetForEvaluationAsync(string code, Guid userId, CancellationToken cancellationToken = default);

    Task<GetCouponByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}