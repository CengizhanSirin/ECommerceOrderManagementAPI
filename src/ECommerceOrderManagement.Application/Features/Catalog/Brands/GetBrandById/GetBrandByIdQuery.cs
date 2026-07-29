using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;

public sealed record GetBrandByIdQuery(Guid BrandId) : IQuery<GetBrandByIdResponse>
{
}