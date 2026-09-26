using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;
using ECommerceOrderManagement.Domain.Orders;
using ECommerceOrderManagement.Domain.Payments;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.Payments;

public sealed class ProcessPaymentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldMarkOrderAsPaidAndSaveChanges_WhenPaymentSucceeds()
    {
        // Arrange
        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var paymentServiceMock = new Mock<IPaymentService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var currentUserMock = new Mock<ICurrentUser>();

        var userId = Guid.NewGuid();

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
            userId,
            "ORD-001",
            address,
            address,
            items);

        var command = new ProcessPaymentCommand(
            OrderId: order.Id,
            CardHolderName: "Test Customer",
            CardNumber: "4111111111111111",
            ExpireMonth: "12",
            ExpireYear: "2030",
            Cvc: "123");

        currentUserMock.Setup(user => user.UserId).Returns(userId);

        orderRepositoryMock
            .Setup(repository => repository.GetByIdAndCustomerIdWithItemsAsync(order.Id, userId, CancellationToken.None))
            .ReturnsAsync(order);

        paymentRepositoryMock
            .Setup(repository => repository.ExistsSuccessfulPaymentByOrderIdAsync(order.Id, CancellationToken.None))
            .ReturnsAsync(false);

        paymentServiceMock
            .Setup(service => service.ProviderName)
            .Returns("TestProvider");

        paymentServiceMock
            .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>(), CancellationToken.None))
            .ReturnsAsync(new PaymentResult(true, "payment-001", null));

        paymentRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Payment>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new ProcessPaymentCommandHandler(
            paymentRepositoryMock.Object,
            paymentServiceMock.Object,
            unitOfWorkMock.Object,
            orderRepositoryMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Paid, order.Status);
        Assert.NotEqual(Guid.Empty, result.Value.PaymentId);
        Assert.Equal("payment-001", result.Value.ProviderPaymentId);

        paymentServiceMock.Verify(

            service => service.ProcessPaymentAsync(

                It.Is<PaymentRequest>(request => request.OrderId == order.Id && request.Amount == order.TotalAmount),
                CancellationToken.None), Times.Once());

        paymentRepositoryMock.Verify(

            repository => repository.AddAsync(

                It.Is<Payment>(payment =>

                    payment.Id == result.Value.PaymentId &&
                    payment.OrderId == order.Id &&
                    payment.Amount == order.TotalAmount &&
                    payment.Status == PaymentStatus.Succeeded &&
                    payment.ProviderPaymentId == "payment-001"),

                CancellationToken.None), Times.Once());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldSaveFailedPaymentAndReturnFailure_WhenPaymentIsDeclined()
    {
        // Arrange
        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var paymentServiceMock = new Mock<IPaymentService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var currentUserMock = new Mock<ICurrentUser>();

        var userId = Guid.NewGuid();

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
            userId,
            "ORD-001",
            address,
            address,
            items);

        var command = new ProcessPaymentCommand(
            OrderId: order.Id,
            CardHolderName: "Test Customer",
            CardNumber: "4111111111111111",
            ExpireMonth: "12",
            ExpireYear: "2030",
            Cvc: "123");

        var failureReason = "Payment was declined.";

        currentUserMock.Setup(user => user.UserId).Returns(userId);

        orderRepositoryMock
            .Setup(repository => repository.GetByIdAndCustomerIdWithItemsAsync(order.Id, userId, CancellationToken.None))
            .ReturnsAsync(order);

        paymentRepositoryMock
            .Setup(repository => repository.ExistsSuccessfulPaymentByOrderIdAsync(order.Id, CancellationToken.None))
            .ReturnsAsync(false);

        paymentServiceMock
            .Setup(service => service.ProviderName)
            .Returns("TestProvider");

        paymentServiceMock
            .Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>(), CancellationToken.None))
            .ReturnsAsync(new PaymentResult(false, null, failureReason));

        paymentRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Payment>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new ProcessPaymentCommandHandler(
            paymentRepositoryMock.Object,
            paymentServiceMock.Object,
            unitOfWorkMock.Object,
            orderRepositoryMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Payment.Failed", result.Error.Code);
        Assert.Equal(OrderStatus.Pending, order.Status);

        paymentRepositoryMock.Verify(

            repository => repository.AddAsync(

                It.Is<Payment>(payment =>
                    payment.OrderId == order.Id &&
                    payment.Amount == order.TotalAmount &&
                    payment.Status == PaymentStatus.Failed &&
                    payment.FailureReason == failureReason),

                CancellationToken.None), Times.Once());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureWithoutCallingPaymentService_WhenOrderAlreadyPaid()
    {
        // Arrange
        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var paymentServiceMock = new Mock<IPaymentService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var currentUserMock = new Mock<ICurrentUser>();

        var userId = Guid.NewGuid();

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
            userId,
            "ORD-001",
            address,
            address,
            items);

        order.MarkAsPaid();

        var command = new ProcessPaymentCommand(
            OrderId: order.Id,
            CardHolderName: "Test Customer",
            CardNumber: "4111111111111111",
            ExpireMonth: "12",
            ExpireYear: "2030",
            Cvc: "123");

        currentUserMock
            .Setup(user => user.UserId)
            .Returns(userId);

        orderRepositoryMock
            .Setup(repository => repository.GetByIdAndCustomerIdWithItemsAsync(order.Id, userId, CancellationToken.None))
            .ReturnsAsync(order);

        paymentRepositoryMock
            .Setup(repository => repository.ExistsSuccessfulPaymentByOrderIdAsync(order.Id, CancellationToken.None))
            .ReturnsAsync(true);

        var handler = new ProcessPaymentCommandHandler(
            paymentRepositoryMock.Object,
            paymentServiceMock.Object,
            unitOfWorkMock.Object,
            orderRepositoryMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Payment.AlreadyPaid", result.Error.Code);

        paymentServiceMock.Verify(

            service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>(), CancellationToken.None),
            Times.Never());

        paymentRepositoryMock.Verify(

            repository => repository.AddAsync(It.IsAny<Payment>(), CancellationToken.None),
            Times.Never());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}