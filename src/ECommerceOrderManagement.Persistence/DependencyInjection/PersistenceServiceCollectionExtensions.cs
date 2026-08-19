using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Persistence.Contexts;
using ECommerceOrderManagement.Persistence.Interceptors;
using ECommerceOrderManagement.Persistence.Repositories.Addresses;
using ECommerceOrderManagement.Persistence.Repositories.Addresses.Queries;
using ECommerceOrderManagement.Persistence.Repositories.Catalog;
using ECommerceOrderManagement.Persistence.Repositories.Catalog.Queries;
using ECommerceOrderManagement.Persistence.Repositories.Inventory;
using ECommerceOrderManagement.Persistence.Repositories.Inventory.Queries;
using ECommerceOrderManagement.Persistence.Repositories.Orders;
using ECommerceOrderManagement.Persistence.Repositories.Orders.Queries;
using ECommerceOrderManagement.Persistence.Repositories.ShoppingCarts;
using ECommerceOrderManagement.Persistence.Repositories.ShoppingCarts.Queries;
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
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IUserAddressPreferenceRepository, UserAddressPreferenceRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IBrandQueries, BrandQueries>();
        services.AddScoped<IInventoryQueries, InventoryQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<IAddressQueries, AddressQueries>();
        services.AddScoped<IShoppingCartQueries, ShoppingCartQueries>();

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