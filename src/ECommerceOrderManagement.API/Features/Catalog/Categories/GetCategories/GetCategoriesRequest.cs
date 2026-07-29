namespace ECommerceOrderManagement.API.Features.Catalog.Categories.GetCategories;

public sealed class GetCategoriesRequest
{
    public string? SearchTerm { get; init; }

    public bool? IsActive { get; init; }
}