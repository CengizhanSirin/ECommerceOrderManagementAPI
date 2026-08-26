using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.API.Features.Coupons.GetCoupons;

public sealed record GetCouponsRequest(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    DiscountType? DiscountType = null,
    bool? IsActive = null,
    string? SortBy = null,
    string? SortDirection = null);