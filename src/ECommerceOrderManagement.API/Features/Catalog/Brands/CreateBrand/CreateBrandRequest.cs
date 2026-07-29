namespace ECommerceOrderManagement.API.Features.Catalog.Brands.CreateBrand;

public sealed class CreateBrandRequest
{
    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? LogoUrl { get; init; }
}