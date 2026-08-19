using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public static class ProductErrors
{
    public static Error SkuAlreadyExists(string sku)
    {
        return Error.Conflict("Product.SkuAlreadyExists", $"A product with SKU '{sku}' already exists.");
    }

    public static Error SlugAlreadyExists(string slug)
    {
        return Error.Conflict("Product.SlugAlreadyExists", $"A product with slug '{slug}' already exists.");
    }

    public static Error NotFound(Guid productId)
    {
        return Error.NotFound("Product.NotFound", $"Product with ID '{productId}' was not found.");
    }

    public static Error Inactive(Guid productId)
    {
        return Error.Failure("Product.Inactive", $"Product with ID '{productId}' is inactive.");
    }
}