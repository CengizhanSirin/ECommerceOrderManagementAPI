using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Infrastructure.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceOrderManagement.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOrderNumberGenerator, OrderNumberGenerator>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }
}