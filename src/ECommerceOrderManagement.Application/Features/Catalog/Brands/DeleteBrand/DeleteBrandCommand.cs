using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.DeleteBrand;

public sealed record DeleteBrandCommand(Guid BrandId) : ICommand
{
}