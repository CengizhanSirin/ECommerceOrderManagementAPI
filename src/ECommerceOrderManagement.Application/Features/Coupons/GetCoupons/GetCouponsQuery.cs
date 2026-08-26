using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;

public sealed record GetCouponsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    DiscountType? DiscountType,
    bool? IsActive,
    string? SortBy,
    string? SortDirection)
    : IQuery<PagedResult<GetCouponsItemResponse>>;