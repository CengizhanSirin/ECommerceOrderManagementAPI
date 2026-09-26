using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.Inventory.ReserveStock;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Domain.Inventory;
using ECommerceOrderManagement.Domain.ValueObjects;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.Inventory;

public sealed class ReserveStockCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReserveStockAndSaveChanges_WhenStockIsSufficient()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var inventoryRepositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var product = Product.Create(
            name: "Test Product",
            slug: "test-product",
            sku: "TEST-001",
            price: Money.Create(100m, "TRY"),
            categoryId: Guid.NewGuid());

        var inventoryItem = InventoryItem.Create(product.Id, 10, 2);

        productRepositoryMock.Setup(repository => repository.GetByIdAsync(product.Id, CancellationToken.None)).ReturnsAsync(product);

        inventoryRepositoryMock.Setup(repository => repository.GetByProductIdAsync(product.Id, CancellationToken.None)).ReturnsAsync(inventoryItem);

        unitOfWorkMock.Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

        var handler = new ReserveStockCommandHandler(
            productRepositoryMock.Object,
            inventoryRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(product.Id, 3, "Test reservation");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, inventoryItem.ReservedQuantity);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var inventoryRepositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var productId = Guid.NewGuid();

        productRepositoryMock.Setup(repository => repository.GetByIdAsync(productId, CancellationToken.None)).ReturnsAsync((Product?)null);

        var handler = new ReserveStockCommandHandler(
            productRepositoryMock.Object,
            inventoryRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(productId, 3, "Test reservation");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.ProductNotFound", result.Error.Code);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenStockIsInsufficient()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var inventoryRepositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var product = Product.Create(
            name: "Test Product",
            slug: "test-product",
            sku: "TEST-001",
            price: Money.Create(100m, "TRY"),
            categoryId: Guid.NewGuid());

        var inventoryItem = InventoryItem.Create(product.Id, 10, 2); inventoryItem.ReserveStock(6);

        productRepositoryMock.Setup(repository => repository.GetByIdAsync(product.Id, CancellationToken.None)).ReturnsAsync(product);

        inventoryRepositoryMock.Setup(repository => repository.GetByProductIdAsync(product.Id, CancellationToken.None)).ReturnsAsync(inventoryItem);

        var handler = new ReserveStockCommandHandler(
            productRepositoryMock.Object,
            inventoryRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(product.Id, 5, "Test reservation");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.InsufficientStock", result.Error.Code);
        Assert.Equal(6, inventoryItem.ReservedQuantity);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}