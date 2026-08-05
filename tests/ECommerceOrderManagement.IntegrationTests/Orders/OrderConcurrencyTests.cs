using ECommerceOrderManagement.Application.Common.Exceptions;
using ECommerceOrderManagement.Domain.Orders;
using ECommerceOrderManagement.Persistence.Contexts;
using ECommerceOrderManagement.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.IntegrationTests.Orders;

public sealed class OrderConcurrencyTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldThrowConcurrencyConflictException_WhenOrderWasModifiedByAnotherContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING");

        Assert.False(string.IsNullOrWhiteSpace(connectionString), "ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING is not configured.");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;

        Guid? orderId = null;

        try
        {
            orderId = await CreateTestOrderAsync(options);

            await using var contextA = new ApplicationDbContext(options);

            await using var contextB = new ApplicationDbContext(options);

            await using var unitOfWorkA = new UnitOfWork(contextA);

            await using var unitOfWorkB = new UnitOfWork(contextB);



            var orderA = await contextA.Orders.SingleAsync(order => order.Id == orderId);

            var orderB = await contextB.Orders.SingleAsync(order => order.Id == orderId);



            const string firstCancellationReason = "Cancelled by the first context.";

            const string secondCancellationReason = "Cancelled by the second context.";

            orderA.Cancel(firstCancellationReason);

            await unitOfWorkA.SaveChangesAsync();

            orderB.Cancel(secondCancellationReason);

            var exception = await Assert.ThrowsAsync<ConcurrencyConflictException>(async () => await unitOfWorkB.SaveChangesAsync());

            Assert.IsType<DbUpdateConcurrencyException>(exception.InnerException);

            await using var verificationContext = new ApplicationDbContext(options);

            var persistedOrder = await verificationContext.Orders
                .AsNoTracking()
                .Where(order => order.Id == orderId)
                .Select(order => new
                {
                    order.Status,
                    order.CancellationReason
                })
                .SingleAsync();

            Assert.Equal(OrderStatus.Cancelled, persistedOrder.Status);

            Assert.Equal(firstCancellationReason, persistedOrder.CancellationReason);
        }
        finally
        {
            if (orderId.HasValue)
            {
                await using var cleanupContext = new ApplicationDbContext(options);

                await cleanupContext.Orders.Where(order => order.Id == orderId.Value).ExecuteDeleteAsync();
            }
        }
    }

    private static async Task<Guid> CreateTestOrderAsync(DbContextOptions<ApplicationDbContext> options)
    {
        var shippingAddress = OrderAddress.Create(
            "Concurrency Test Customer",
            "05550000001",
            "Türkiye",
            "İstanbul",
            "Kadıköy",
            "34710",
            "Concurrency Test Shipping Address");

        var billingAddress = OrderAddress.Create(
            "Concurrency Test Customer",
            "05550000001",
            "Türkiye",
            "İstanbul",
            "Kadıköy",
            "34710",
            "Concurrency Test Billing Address");

        var itemSnapshots = new[]
        {
            new OrderItemSnapshot(
                Guid.NewGuid(),
                "Concurrency Test Product",
                $"TEST-{Guid.NewGuid():N}",
                100m,
                1)
        };

        var order = Order.Create(
            Guid.NewGuid(),
            $"TEST-ORDER-{Guid.NewGuid():N}",
            shippingAddress,
            billingAddress,
            itemSnapshots);

        await using var setupContext = new ApplicationDbContext(options);

        await setupContext.Orders.AddAsync(order);

        await setupContext.SaveChangesAsync();

        return order.Id;
    }

}