using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name, string Slug, string? Description, string? ImageUrl, int DisplayOrder)
: ICommand<CreateCategoryResponse>
{
}