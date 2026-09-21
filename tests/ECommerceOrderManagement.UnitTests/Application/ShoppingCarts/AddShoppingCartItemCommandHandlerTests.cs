using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Domain.Inventory;
using ECommerceOrderManagement.Domain.ShoppingCarts;
using ECommerceOrderManagement.Domain.ValueObjects;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.ShoppingCarts;

public sealed class AddShoppingCartItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCartAndSaveChanges_WhenUserHasNoCart()
    {
        // Arrange
        var shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
        var productRepositoryMock = new Mock<IProductRepository>();
        var inventoryRepositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var currentUserMock = new Mock<ICurrentUser>();

        var userId = Guid.NewGuid();

        var product = Product.Create(
            "Test Product",
            "test-product",
            "TEST-001",
            Money.Create(100m, "TRY"),
            Guid.NewGuid());

        var inventoryItem = InventoryItem.Create(product.Id, 10, 2);
        var command = new AddShoppingCartItemCommand(product.Id, 3);

        currentUserMock.Setup(user => user.UserId).Returns(userId);

        productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(product.Id, CancellationToken.None))
            .ReturnsAsync(product);

        shoppingCartRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((ShoppingCart?)null);

        inventoryRepositoryMock
            .Setup(repository => repository.GetByProductIdAsync(product.Id, CancellationToken.None))
            .ReturnsAsync(inventoryItem);

        shoppingCartRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<ShoppingCart>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new AddShoppingCartItemCommandHandler(
            shoppingCartRepositoryMock.Object,
            productRepositoryMock.Object,
            inventoryRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        shoppingCartRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<ShoppingCart>(cart =>
                    cart.UserId == userId &&
                    cart.Items.Count == 1 &&
                    cart.Items.Any(item =>
                    item.ProductId == product.Id && item.Quantity == command.Quantity)),
                CancellationToken.None), Times.Once());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTotalQuantityExceedsAvailableStock()
    {
        // Arrange
        var shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>();
        var productRepositoryMock = new Mock<IProductRepository>();
        var inventoryRepositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var currentUserMock = new Mock<ICurrentUser>();

        var userId = Guid.NewGuid();

        var product = Product.Create(
            "Test Product",
            "test-product",
            "TEST-001",
            Money.Create(100m, "TRY"),
            Guid.NewGuid());

        var inventoryItem = InventoryItem.Create(product.Id, 10, 2);

        var shoppingCart = ShoppingCart.Create(userId);

        shoppingCart.TryAddItem(product.Id, 6);

        var command = new AddShoppingCartItemCommand(product.Id, 5);

        currentUserMock
            .Setup(user => user.UserId)
            .Returns(userId);

        productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(product.Id, CancellationToken.None))
            .ReturnsAsync(product);

        shoppingCartRepositoryMock
            .Setup(repository => repository.GetByUserIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(shoppingCart);

        inventoryRepositoryMock
            .Setup(repository => repository.GetByProductIdAsync(product.Id, CancellationToken.None))
            .ReturnsAsync(inventoryItem);

        var handler = new AddShoppingCartItemCommandHandler(
            shoppingCartRepositoryMock.Object,
            productRepositoryMock.Object,
            inventoryRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Inventory.InsufficientStock", result.Error.Code);

        var item = Assert.Single(shoppingCart.Items);
        Assert.Equal(6, item.Quantity);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}