namespace ECommerceOrderManagement.API.Features.Catalog.Categories.CreateCategory;

public sealed class CreateCategoryRequest
{
    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? ImageUrl { get; init; }

    public int DisplayOrder { get; init; }
}