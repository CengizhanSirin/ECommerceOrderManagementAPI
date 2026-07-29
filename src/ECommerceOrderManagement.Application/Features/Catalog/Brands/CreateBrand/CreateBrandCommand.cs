using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.CreateBrand;

public sealed record CreateBrandCommand(string Name, string Slug, string? Description, string? LogoUrl) : ICommand<CreateBrandResponse>
{
}