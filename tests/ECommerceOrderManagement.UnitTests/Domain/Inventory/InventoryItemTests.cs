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

}