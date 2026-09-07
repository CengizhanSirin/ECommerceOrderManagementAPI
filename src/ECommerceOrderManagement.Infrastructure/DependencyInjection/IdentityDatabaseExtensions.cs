using ECommerceOrderManagement.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceOrderManagement.Infrastructure.DependencyInjection;

public static class IdentityDatabaseExtensions
{
    public static async Task ApplyIdentityMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}