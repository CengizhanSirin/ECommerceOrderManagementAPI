namespace ECommerceOrderManagement.Application.Features.Catalog.Brands;

internal static class BrandValidationErrors
{
    public const string IdRequiredCode = "Brands.Id.Required";
    public const string IdRequiredMessage = "Brand ID is required.";
    public const string NameRequiredCode = "Brands.Name.Required";
    public const string NameRequiredMessage = "Brand name is required.";
    public const string NameMaxLengthCode = "Brands.Name.MaxLength";
    public const string SlugRequiredCode = "Brands.Slug.Required";
    public const string SlugRequiredMessage = "Brand slug is required.";
    public const string SlugMaxLengthCode = "Brands.Slug.MaxLength";
    public const string SlugInvalidFormatCode = "Brands.Slug.InvalidFormat";
    public const string SlugInvalidFormatMessage = "Brand slug may contain only letters, numbers, and hyphens.";
    public const string DescriptionMaxLengthCode = "Brands.Description.MaxLength";
    public const string LogoUrlMaxLengthCode = "Brands.LogoUrl.MaxLength";
    public const string LogoUrlInvalidCode = "Brands.LogoUrl.Invalid";
    public const string LogoUrlInvalidMessage = "Logo URL must be a valid HTTP or HTTPS URL.";
}