using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Entities;

public sealed class Brand : SoftDeletableAggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? LogoUrl { get; private set; }

    public bool IsActive { get; private set; }

    private Brand()
    {
    }

    private Brand(string name, string slug, string? description, string? logoUrl)
    {
        Name = name;
        Slug = slug;
        Description = description;
        LogoUrl = logoUrl;
        IsActive = true;
    }

    public static Brand Create(string name, string slug, string? description = null, string? logoUrl = null)

    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        return new Brand(
            name.Trim(),
            slug.Trim().ToLowerInvariant(),
            NormalizeOptionalText(description),
            NormalizeOptionalText(logoUrl));
    }

    public void Update(string name, string slug, string? description, string? logoUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        Name = name.Trim();
        Slug = slug.Trim().ToLowerInvariant();
        Description = NormalizeOptionalText(description);
        LogoUrl = NormalizeOptionalText(logoUrl);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
