using ECommerceOrderManagement.Domain.Inventory;

namespace ECommerceOrderManagement.UnitTests.Domain.Inventory;

public sealed class InventoryItemTests
{
    [Fact]
    public void ReserveStock_ShouldReserveQuantity_WhenStockIsSufficient()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        // Act
        inventoryItem.ReserveStock(3);

        // Assert
        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(3, inventoryItem.ReservedQuantity);
        Assert.Equal(7, inventoryItem.AvailableQuantity);

        var stockMovement = Assert.Single(inventoryItem.StockMovements);

        Assert.Equal(StockMovementType.Reserve, stockMovement.Type);
        Assert.Equal(3, stockMovement.Quantity);
    }

    [Fact]
    public void ReserveStock_ShouldThrowInvalidOperationException_WhenQuantityExceedsAvailableStock()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(4);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => inventoryItem.ReserveStock(7));

        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(4, inventoryItem.ReservedQuantity);
        Assert.Equal(6, inventoryItem.AvailableQuantity);

        Assert.Single(inventoryItem.StockMovements);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReserveStock_ShouldThrowArgumentOutOfRangeException_WhenQuantityIsNotPositive(int quantity)
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => inventoryItem.ReserveStock(quantity));

        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(0, inventoryItem.ReservedQuantity);
        Assert.Equal(10, inventoryItem.AvailableQuantity);

        Assert.Empty(inventoryItem.StockMovements);
    }

    [Fact]
    public void ReserveStock_ShouldReserveAllAvailableStock_WhenQuantityEqualsAvailableStock()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(4);

        // Act
        inventoryItem.ReserveStock(6);

        // Assert
        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(10, inventoryItem.ReservedQuantity);
        Assert.Equal(0, inventoryItem.AvailableQuantity);

        Assert.Equal(2, inventoryItem.StockMovements.Count);
    }

    [Fact]
    public void ReleaseStock_ShouldReleaseQuantity_WhenReservedStockIsSufficient()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(6);

        // Act
        inventoryItem.ReleaseStock(2);

        // Assert
        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(4, inventoryItem.ReservedQuantity);
        Assert.Equal(6, inventoryItem.AvailableQuantity);

        Assert.Equal(2, inventoryItem.StockMovements.Count);

        var releaseMovement = Assert.Single(
            inventoryItem.StockMovements,
            movement => movement.Type == StockMovementType.Release);

        Assert.Equal(2, releaseMovement.Quantity);
        Assert.Equal(6, releaseMovement.ReservedQuantityBefore);
        Assert.Equal(4, releaseMovement.ReservedQuantityAfter);
    }

    [Fact]
    public void ReleaseStock_ShouldThrowInvalidOperationException_WhenQuantityExceedsReservedStock()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(4);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => inventoryItem.ReleaseStock(5));

        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(4, inventoryItem.ReservedQuantity);
        Assert.Equal(6, inventoryItem.AvailableQuantity);

        var stockMovement = Assert.Single(inventoryItem.StockMovements);

        Assert.Equal(StockMovementType.Reserve, stockMovement.Type);
        Assert.Equal(4, stockMovement.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReleaseStock_ShouldThrowArgumentOutOfRangeException_WhenQuantityIsNotPositive(int quantity)
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(4);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => inventoryItem.ReleaseStock(quantity));

        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(4, inventoryItem.ReservedQuantity);
        Assert.Equal(6, inventoryItem.AvailableQuantity);

        var stockMovement = Assert.Single(inventoryItem.StockMovements);

        Assert.Equal(StockMovementType.Reserve, stockMovement.Type);
        Assert.Equal(4, stockMovement.Quantity);
    }

    [Fact]
    public void ReleaseStock_ShouldReleaseAllReservedStock_WhenQuantityEqualsReservedStock()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(4);

        // Act
        inventoryItem.ReleaseStock(4);

        // Assert
        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(0, inventoryItem.ReservedQuantity);
        Assert.Equal(10, inventoryItem.AvailableQuantity);

        Assert.Equal(2, inventoryItem.StockMovements.Count);

        var releaseMovement = Assert.Single(
            inventoryItem.StockMovements,
            movement => movement.Type == StockMovementType.Release);

        Assert.Equal(4, releaseMovement.Quantity);
        Assert.Equal(4, releaseMovement.ReservedQuantityBefore);
        Assert.Equal(0, releaseMovement.ReservedQuantityAfter);
    }

    [Fact]
    public void DecreaseStock_ShouldThrowInvalidOperationException_WhenQuantityExceedsAvailableStock()
    {
        // Arrange
        var inventoryItem = InventoryItem.Create(
            productId: Guid.NewGuid(),
            initialQuantity: 10,
            reorderLevel: 2);

        inventoryItem.ReserveStock(6);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => inventoryItem.DecreaseStock(5));

        Assert.Equal(10, inventoryItem.QuantityOnHand);
        Assert.Equal(6, inventoryItem.ReservedQuantity);

        Assert.Single(inventoryItem.StockMovements);
    }


}