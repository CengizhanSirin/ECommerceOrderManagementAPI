namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;

public sealed record GetCategoryByIdResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }

    public string? ImageUrl { get; init; }

    public int DisplayOrder { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }
}