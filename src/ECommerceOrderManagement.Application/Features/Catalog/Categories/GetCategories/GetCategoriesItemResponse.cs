namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;

public sealed record GetCategoriesItemResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public string? ImageUrl { get; init; }

    public int DisplayOrder { get; init; }
}