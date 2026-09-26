using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.UnitTests.Domain.Orders;

public sealed class OrderTests
{
    [Fact]
    public void Create_ShouldCalculateTotalAmount_WhenItemsAreValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var address = OrderAddress.Create(
            fullName: "Test Customer",
            phoneNumber: "05555555555",
            country: "Türkiye",
            city: "İstanbul",
            district: "Ataşehir",
            postalCode: "34758",
            addressLine: "Test Mahallesi, Test Sokak No: 1");

        var items = new List<OrderItemSnapshot>
        {
            new OrderItemSnapshot(Guid.NewGuid(), "Product A", "SKU-001", 100m, 2),
            new OrderItemSnapshot(Guid.NewGuid(), "Product B", "SKU-002", 50m, 3)
        };

        // Act
        var order = Order.Create(
            customerId,
            "ORD-001",
            address,
            address,
            items);

        // Assert
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(350m, order.Subtotal);
        Assert.Equal(0m, order.DiscountAmount);
        Assert.Equal(350m, order.TotalAmount);
    }

    [Fact]
    public void Create_ShouldThrowInvalidOperationException_WhenItemsAreEmpty()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var address = OrderAddress.Create(
            fullName: "Test Customer",
            phoneNumber: "05555555555",
            country: "Türkiye",
            city: "İstanbul",
            district: "Ataşehir",
            postalCode: "34758",
            addressLine: "Test Mahallesi, Test Sokak No: 1");

        var items = new List<OrderItemSnapshot>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Order.Create(
                customerId,
                "ORD-001",
                address,
                address,
                items));
    }

    [Fact]
    public void StartProcessing_ShouldThrowInvalidOperationException_WhenOrderIsPending()
    {
        // Arrange
        var address = OrderAddress.Create(
            fullName: "Test Customer",
            phoneNumber: "05555555555",
            country: "Türkiye",
            city: "İstanbul",
            district: "Ataşehir",
            postalCode: "34758",
            addressLine: "Test Mahallesi, Test Sokak No: 1");

        var items = new List<OrderItemSnapshot>
    {
        new OrderItemSnapshot(Guid.NewGuid(), "Product A", "SKU-001", 100m, 2)
    };

        var order = Order.Create(
            Guid.NewGuid(),
            "ORD-001",
            address,
            address,
            items);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.StartProcessing());

        Assert.Equal(OrderStatus.Pending, order.Status);
    }
}