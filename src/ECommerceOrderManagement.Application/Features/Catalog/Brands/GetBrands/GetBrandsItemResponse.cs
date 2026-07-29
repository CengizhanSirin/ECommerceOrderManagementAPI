namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;

public sealed record GetBrandsItemResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? LogoUrl { get; init; }

    public bool IsActive { get; init; }
}