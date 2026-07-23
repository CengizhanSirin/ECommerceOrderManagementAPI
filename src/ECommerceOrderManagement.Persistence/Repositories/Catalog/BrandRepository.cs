using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Persistence.Contexts;

namespace ECommerceOrderManagement.Persistence.Repositories.Catalog;

internal sealed class BrandRepository(ApplicationDbContext dbContext) : Repository<Brand>(dbContext), IBrandRepository
{
}