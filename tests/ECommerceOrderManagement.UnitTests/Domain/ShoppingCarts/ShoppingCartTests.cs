using ECommerceOrderManagement.Domain.ShoppingCarts;

namespace ECommerceOrderManagement.UnitTests.Domain.ShoppingCarts;

public sealed class ShoppingCartTests
{
    [Fact]
    public void TryAddItem_ShouldAddItem_WhenProductIsNotInCart()
    {
        // Arrange
        var shoppingCart = ShoppingCart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        var result = shoppingCart.TryAddItem(productId, 2);

        // Assert
        Assert.True(result);

        var item = Assert.Single(shoppingCart.Items);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(shoppingCart.Id, item.ShoppingCartId);
    }

    [Fact]
    public void TryAddItem_ShouldIncreaseQuantity_WhenProductAlreadyExistsInCart()
    {
        // Arrange
        var shoppingCart = ShoppingCart.Create(Guid.NewGuid());
        var productId = Guid.NewGuid();

        shoppingCart.TryAddItem(productId, 2);

        // Act
        var result = shoppingCart.TryAddItem(productId, 3);

        // Assert
        Assert.True(result);

        var item = Assert.Single(shoppingCart.Items);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(5, item.Quantity);
    }

    [Fact]
    public void TryAddItem_ShouldReturnFalse_WhenMaximumDistinctItemLimitIsReached()
    {
        // Arrange
        var shoppingCart = ShoppingCart.Create(Guid.NewGuid());

        for (var i = 0; i < 50; i++)
        {
            shoppingCart.TryAddItem(Guid.NewGuid(), 1);
        }

        var newProductId = Guid.NewGuid();

        // Act
        var result = shoppingCart.TryAddItem(newProductId, 1);

        // Assert
        Assert.False(result);
        Assert.Equal(50, shoppingCart.Items.Count);

        Assert.DoesNotContain(shoppingCart.Items, item => item.ProductId == newProductId);
    }
}