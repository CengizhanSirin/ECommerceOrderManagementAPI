namespace ECommerceOrderManagement.Application.Features.Catalog.Categories;

internal static class CategoryValidationErrors
{
    public const string NameRequiredCode = "Categories.Name.Required";
    public const string NameRequiredMessage = "Category name is required.";
    public const string NameMaxLengthCode = "Categories.Name.MaxLength";
    public const string SlugRequiredCode = "Categories.Slug.Required";
    public const string SlugRequiredMessage = "Category slug is required.";
    public const string SlugMaxLengthCode = "Categories.Slug.MaxLength";
    public const string SlugInvalidFormatCode = "Categories.Slug.InvalidFormat";
    public const string SlugInvalidFormatMessage = "Category slug may contain only letters, numbers, and hyphens.";
    public const string DescriptionMaxLengthCode = "Categories.Description.MaxLength";
    public const string ImageUrlMaxLengthCode = "Categories.ImageUrl.MaxLength";
    public const string ImageUrlInvalidCode = "Categories.ImageUrl.Invalid";
    public const string ImageUrlInvalidMessage = "Image URL must be a valid HTTP or HTTPS URL.";
    public const string DisplayOrderInvalidCode = "Categories.DisplayOrder.Invalid";
    public const string DisplayOrderInvalidMessage = "Display order cannot be negative.";
}