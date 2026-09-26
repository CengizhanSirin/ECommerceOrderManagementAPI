using ECommerceOrderManagement.Domain.Payments;
using ECommerceOrderManagement.Domain.Payments.Events;

namespace ECommerceOrderManagement.UnitTests.Domain.Payments;

public sealed class PaymentTests
{
    [Fact]
    public void MarkAsSucceeded_ShouldUpdateStatusAndRaiseEvent_WhenPaymentIsPending()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var payment = Payment.Create(
            orderId,
            200m,
            PaymentMethod.CreditCard,
            "TestProvider");

        // Act
        payment.MarkAsSucceeded("payment-001");

        // Assert
        Assert.Equal(PaymentStatus.Succeeded, payment.Status);
        Assert.Equal("payment-001", payment.ProviderPaymentId);

        var domainEvent = Assert.Single(payment.DomainEvents);
        var paymentEvent = Assert.IsType<PaymentSucceededDomainEvent>(domainEvent);

        Assert.Equal(payment.Id, paymentEvent.PaymentId);
        Assert.Equal(orderId, paymentEvent.OrderId);
        Assert.Equal(200m, paymentEvent.Amount);
    }

    [Fact]
    public void MarkAsSucceeded_ShouldThrowInvalidOperationException_WhenPaymentAlreadySucceeded()
    {
        // Arrange
        var payment = Payment.Create(
            Guid.NewGuid(),
            200m,
            PaymentMethod.CreditCard,
            "TestProvider");

        payment.MarkAsSucceeded("payment-001");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => payment.MarkAsSucceeded("payment-002"));

        Assert.Equal(PaymentStatus.Succeeded, payment.Status);
        Assert.Equal("payment-001", payment.ProviderPaymentId);
        Assert.Single(payment.DomainEvents);
    }
}