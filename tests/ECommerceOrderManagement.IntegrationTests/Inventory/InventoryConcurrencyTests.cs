using ECommerceOrderManagement.Application.Common.Exceptions;
using ECommerceOrderManagement.Persistence.Contexts;
using ECommerceOrderManagement.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerceOrderManagement.IntegrationTests.Inventory;

public sealed class InventoryConcurrencyTests
{
    private static readonly Guid ProductId = Guid.Parse("6a4d9e38-7f7c-43ae-8391-e3e5f1868d44");

    [Fact]
    public async Task SaveChangesAsync_ShouldThrowConcurrencyConflictException_WhenInventoryWasModifiedByAnotherContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING");

        Assert.False(string.IsNullOrWhiteSpace(connectionString), "ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING is not configured.");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;

        int? originalReorderLevel = null;

        try
        {
            await using var contextA = new ApplicationDbContext(options);

            await using var contextB = new ApplicationDbContext(options);

            await using var unitOfWorkA = new UnitOfWork(contextA);

            await using var unitOfWorkB = new UnitOfWork(contextB);

            var inventoryItemA = await contextA.InventoryItems.SingleAsync(inventoryItem => inventoryItem.ProductId == ProductId);

            var inventoryItemB = await contextB.InventoryItems.SingleAsync(inventoryItem => inventoryItem.ProductId == ProductId);

            originalReorderLevel = inventoryItemA.ReorderLevel;

            var firstReorderLevel = originalReorderLevel == 10
                    ? 11
                    : 10;

            var secondReorderLevel = originalReorderLevel == 20
                    ? 21
                    : 20;

            inventoryItemA.ChangeReorderLevel(firstReorderLevel);

            await unitOfWorkA.SaveChangesAsync();

            inventoryItemB.ChangeReorderLevel(secondReorderLevel);

            var exception = await Assert.ThrowsAsync<ConcurrencyConflictException>(async () => await unitOfWorkB.SaveChangesAsync());

            Assert.IsType<DbUpdateConcurrencyException>(exception.InnerException);

            await using var verificationContext = new ApplicationDbContext(options);

            var persistedReorderLevel = await verificationContext.InventoryItems
                    .AsNoTracking()
                    .Where(inventoryItem => inventoryItem.ProductId == ProductId)
                    .Select(inventoryItem => inventoryItem.ReorderLevel)
                    .SingleAsync();

            Assert.Equal(firstReorderLevel, persistedReorderLevel);
        }
        finally
        {
            if (originalReorderLevel.HasValue)
            {
                await using var restorationContext = new ApplicationDbContext(options);

                var inventoryItem = await restorationContext.InventoryItems.SingleAsync(item => item.ProductId == ProductId);

                inventoryItem.ChangeReorderLevel(originalReorderLevel.Value);

                await restorationContext.SaveChangesAsync();
            }
        }
    }
}