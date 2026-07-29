using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands;

public static class BrandErrors
{
    public static Error NotFound(Guid brandId)
    {
        return Error.NotFound("Brand.NotFound", $"Brand with ID '{brandId}' was not found.");
    }

    public static Error Inactive(Guid brandId)
    {
        return Error.Failure("Brand.Inactive", $"Brand with ID '{brandId}' is inactive.");
    }

    public static Error NameAlreadyExists(string name)
    {
        return Error.Conflict("Brand.NameAlreadyExists", $"A brand with the name '{name}' already exists.");
    }

    public static Error SlugAlreadyExists(string slug)
    {
        return Error.Conflict("Brand.SlugAlreadyExists", $"A brand with the slug '{slug}' already exists.");
    }

    public static Error HasProducts(Guid brandId)
    {
        return Error.Conflict("Brand.HasProducts", $"The brand with ID '{brandId}' cannot be deleted because it contains products.");
    }
}