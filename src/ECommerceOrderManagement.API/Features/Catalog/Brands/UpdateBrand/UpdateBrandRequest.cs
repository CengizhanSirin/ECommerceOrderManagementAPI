namespace ECommerceOrderManagement.API.Features.Catalog.Brands.UpdateBrand;

public sealed class UpdateBrandRequest
{
    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? LogoUrl { get; init; }
}