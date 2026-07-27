namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public static class ProductValidationErrors
{
    public const string IdRequiredCode = "Product.Id.Required";
    public const string IdRequiredMessage = "Product ID is required.";
    public const string NameRequiredCode = "Product.Name.Required";
    public const string NameRequiredMessage = "Product name is required.";
    public const string NameMaxLengthCode = "Product.Name.MaxLength";
    public const string NameMaxLengthMessage = "Product name must not exceed 200 characters.";
    public const string SlugRequiredCode = "Product.Slug.Required";
    public const string SlugRequiredMessage = "Product slug is required.";
    public const string SlugMaxLengthCode = "Product.Slug.MaxLength";
    public const string SlugMaxLengthMessage = "Product slug must not exceed 220 characters.";
    public const string SkuRequiredCode = "Product.Sku.Required";
    public const string SkuRequiredMessage = "Product SKU is required.";
    public const string SkuMaxLengthCode = "Product.Sku.MaxLength";
    public const string SkuMaxLengthMessage = "Product SKU must not exceed 100 characters.";
    public const string PriceInvalidCode = "Product.Price.Invalid";
    public const string PriceInvalidMessage = "Product price cannot be negative.";
    public const string CurrencyRequiredCode = "Product.Currency.Required";
    public const string CurrencyRequiredMessage = "Product currency is required.";
    public const string CurrencyLengthCode = "Product.Currency.Length";
    public const string CurrencyLengthMessage = "Product currency must contain exactly 3 characters.";
    public const string CategoryRequiredCode = "Product.Category.Required";
    public const string CategoryRequiredMessage = "Product category is required.";
    public const string BrandInvalidCode = "Product.Brand.Invalid";
    public const string BrandInvalidMessage = "Product brand identifier is invalid.";
    public const string DescriptionMaxLengthCode = "Product.Description.MaxLength";
    public const string DescriptionMaxLengthMessage = "Product description must not exceed 4000 characters.";
    public const string MainImageUrlMaxLengthCode = "Product.MainImageUrl.MaxLength";
    public const string MainImageUrlMaxLengthMessage = "Product main image URL must not exceed 2048 characters.";
    public const string ProductsPageNumberInvalidCode = "Products.PageNumber.Invalid";
    public const string ProductsPageNumberInvalidMessage = "Page number must be greater than zero.";
    public const string ProductsSortFieldInvalidCode = "Products.SortField.Invalid";
    public const string ProductsSortFieldInvalidMessage = "The selected product sort field is invalid.";
    public const string ProductsSortDirectionInvalidCode = "Products.SortDirection.Invalid";
    public const string ProductsSortDirectionInvalidMessage = "The selected sort direction is invalid.";
    public const string ProductsCategoryInvalidCode = "Products.Category.Invalid";
    public const string ProductsCategoryInvalidMessage = "Category ID cannot be empty.";
    public const string ProductsPageSizeInvalidCode = "Products.PageSize.Invalid";
    public const string ProductsSearchTermMaxLengthCode = "Products.SearchTerm.MaxLength";
}