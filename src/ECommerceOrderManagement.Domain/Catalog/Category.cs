using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Catalog;

public sealed class Category : SoftDeletableAggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public string? ImageUrl { get; private set; }

    public int DisplayOrder { get; private set; }

    private Category()
    {
    }

    private Category(string name, string slug, string? description, string? imageUrl, int displayOrder)
    {
        Name = name;
        Slug = slug;
        Description = description;
        ImageUrl = imageUrl;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public static Category Create(string name, string slug, string? description = null, string? imageUrl = null, int displayOrder = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        EnsureValidDisplayOrder(displayOrder);

        return new Category(
            name.Trim(),
            slug.Trim().ToLowerInvariant(),
            NormalizeDescription(description),
            NormalizeImageUrl(imageUrl),
            displayOrder);
    }

    public void Update(string name, string slug, string? description, string? imageUrl, int displayOrder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);
        EnsureValidDisplayOrder(displayOrder);

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = NormalizeDescription(description);
        ImageUrl = NormalizeImageUrl(imageUrl);
        DisplayOrder = displayOrder;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static string? NormalizeDescription(string? description)
    {
        return string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();
    }

    private static string? NormalizeImageUrl(string? imageUrl)
    {
        return string.IsNullOrWhiteSpace(imageUrl)
            ? null
            : imageUrl.Trim();
    }

    private static void EnsureValidDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(displayOrder), "Display order cannot be negative.");
        }
    }
}
