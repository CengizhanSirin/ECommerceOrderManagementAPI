namespace ECommerceOrderManagement.API.Features.Catalog.Products.UpdateProduct;

public sealed class UpdateProductRequest
{
    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal PriceAmount { get; init; }

    public string Currency { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }

    public Guid? BrandId { get; init; }

    public string? Description { get; init; }

    public string? MainImageUrl { get; init; }

}