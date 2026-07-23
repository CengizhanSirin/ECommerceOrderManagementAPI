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
}