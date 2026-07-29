using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.UpdateBrand;


public sealed record UpdateBrandCommand(Guid BrandId, string Name, string Slug, string? Description, string? LogoUrl) : ICommand
{
}