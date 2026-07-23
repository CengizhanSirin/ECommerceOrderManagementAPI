using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Persistence.Contexts;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
}