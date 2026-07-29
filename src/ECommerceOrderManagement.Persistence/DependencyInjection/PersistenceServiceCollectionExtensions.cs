using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Persistence.Contexts;
using ECommerceOrderManagement.Persistence.Interceptors;
using ECommerceOrderManagement.Persistence.Repositories.Catalog;
using ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceOrderManagement.Persistence.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

        services.AddSingleton<TimeProvider>(TimeProvider.System);

        services.AddScoped<AuditAndSoftDeleteInterceptor>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();

        services.AddScoped<IUnitOfWork, ECommerceOrderManagement.Persistence.UnitOfWork.UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options)
            =>
            {
                var interceptor = serviceProvider.GetRequiredService<AuditAndSoftDeleteInterceptor>();

                options.UseSqlServer(connectionString).AddInterceptors(interceptor);
            });

        return services;
    }
}