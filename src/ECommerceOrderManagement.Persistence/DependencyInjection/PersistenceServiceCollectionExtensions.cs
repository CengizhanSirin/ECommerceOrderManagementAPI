using ECommerceOrderManagement.Persistence.Contexts;
using ECommerceOrderManagement.Persistence.Interceptors;
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

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options)
            =>
            {
                var interceptor = serviceProvider.GetRequiredService<AuditAndSoftDeleteInterceptor>();

                options.UseSqlServer(connectionString).AddInterceptors(interceptor);
            });

        return services;
    }
}