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

    public static Error NameAlreadyExists(string name)
    {
        return Error.Conflict("Category.NameAlreadyExists", $"A category with the name '{name}' already exists.");
    }

    public static Error SlugAlreadyExists(string slug)
    {
        return Error.Conflict("Category.SlugAlreadyExists", $"A category with the slug '{slug}' already exists.");
    }

    public static Error HasProducts(Guid categoryId)
    {
        return Error.Conflict("Category.HasProducts", $"The category with ID '{categoryId}' cannot be deleted because it contains products.");
    }
}