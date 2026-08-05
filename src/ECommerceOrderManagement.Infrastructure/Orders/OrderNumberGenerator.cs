using ECommerceOrderManagement.Application.Features.Orders;

namespace ECommerceOrderManagement.Infrastructure.Orders;

internal sealed class OrderNumberGenerator(TimeProvider timeProvider) : IOrderNumberGenerator
{
    public string Generate()
    {
        var currentDate = timeProvider.GetUtcNow().ToString("yyyyMMdd");

        var uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

        return $"ORD-{currentDate}-{uniquePart}";
    }
}