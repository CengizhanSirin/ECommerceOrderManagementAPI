using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;
using ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;

namespace ECommerceOrderManagement.Application.Features.Coupons;

public interface ICouponQueries
{
    Task<CouponEvaluationReadModel?> GetForEvaluationAsync(string code, Guid userId, CancellationToken cancellationToken = default);

    Task<GetCouponByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<GetCouponsItemResponse>> GetPagedAsync(GetCouponsQuery query, CancellationToken cancellationToken = default);
}