using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid CategoryId, string Name, string Slug, string? Description, string? ImageUrl, int DisplayOrder) : ICommand
{
}