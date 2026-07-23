using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId)
    {
        return Error.NotFound("Category.NotFound", $"Category with ID '{categoryId}' was not found.");
    }

    public static Error Inactive(Guid categoryId)
    {
        return Error.Failure("Category.Inactive", $"Category with ID '{categoryId}' is inactive.");
    }
}