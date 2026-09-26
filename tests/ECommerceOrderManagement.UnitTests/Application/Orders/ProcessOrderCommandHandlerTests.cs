using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Application.Features.Orders.ProcessOrder;
using ECommerceOrderManagement.Domain.Orders;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.Orders;

public sealed class ProcessOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldStartProcessingAndSaveChanges_WhenOrderIsPaid()
    {
        // Arrange
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

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

        order.MarkAsPaid();

        var command = new ProcessOrderCommand(order.Id);

        orderRepositoryMock
            .Setup(repository => repository.GetByIdWithItemsAsync(order.Id, CancellationToken.None))
            .ReturnsAsync(order);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new ProcessOrderCommandHandler(orderRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Processing, order.Status);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOrderIsPending()
    {
        // Arrange
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

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
        new OrderItemSnapshot( Guid.NewGuid(), "Product A", "SKU-001", 100m, 2)
    };

        var order = Order.Create(
            Guid.NewGuid(),
            "ORD-001",
            address,
            address,
            items);

        var command = new ProcessOrderCommand(order.Id);

        orderRepositoryMock
            .Setup(repository => repository.GetByIdWithItemsAsync(order.Id, CancellationToken.None))
            .ReturnsAsync(order);

        var handler = new ProcessOrderCommandHandler(
            orderRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Orders.CannotStartProcessing", result.Error.Code);
        Assert.Equal(OrderStatus.Pending, order.Status);

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}