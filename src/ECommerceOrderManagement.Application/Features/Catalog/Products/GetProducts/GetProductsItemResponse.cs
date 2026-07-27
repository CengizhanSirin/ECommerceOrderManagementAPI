namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

public sealed record GetProductsItemResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string Sku { get; init; } = string.Empty;

    public decimal PriceAmount { get; init; }

    public string Currency { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public Guid? BrandId { get; init; }

    public string? BrandName { get; init; }

    public bool IsActive { get; init; }

    public string? MainImageUrl { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}