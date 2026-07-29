using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;

public sealed record GetBrandsQuery(string? SearchTerm = null, bool? IsActive = null) : IQuery<IReadOnlyCollection<GetBrandsItemResponse>>
{
}