using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;

public sealed record GetCategoriesQuery(string? SearchTerm = null, bool? IsActive = null) : IQuery<IReadOnlyCollection<GetCategoriesItemResponse>>
{
}