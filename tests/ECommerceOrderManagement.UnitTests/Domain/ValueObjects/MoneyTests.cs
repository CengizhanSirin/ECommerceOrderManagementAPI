using ECommerceOrderManagement.Domain.ValueObjects;

namespace ECommerceOrderManagement.UnitTests.Domain.ValueObjects;

public sealed class MoneyTests
{
    [Fact]
    public void Create_ShouldThrowArgumentOutOfRangeException_WhenAmountIsNegative()
    {
        // Arrange
        var amount = -1m;
        var currency = "TRY";

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.Create(amount, currency));
    }
}