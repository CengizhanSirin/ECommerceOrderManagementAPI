namespace ECommerceOrderManagement.API.Features.Catalog.Brands.GetBrands;

public sealed class GetBrandsRequest
{
    public string? SearchTerm { get; init; }

    public bool? IsActive { get; init; }
}