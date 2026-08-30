using ECommerceOrderManagement.Domain.Common;
using ECommerceOrderManagement.Domain.Payments.Events;

namespace ECommerceOrderManagement.Domain.Payments;

public sealed class Payment : AggregateRoot
{
    public Guid OrderId { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentStatus Status { get; private set; }

    public PaymentMethod Method { get; private set; }

    public string Provider { get; private set; } = string.Empty;

    public string? ProviderPaymentId { get; private set; }

    public string? FailureReason { get; private set; }

    private Payment()
    {
    }

    private Payment(Guid id, Guid orderId, decimal amount, PaymentMethod method, string provider) : base(id)
    {
        OrderId = orderId;
        Amount = amount;
        Method = method;
        Provider = provider;
        Status = PaymentStatus.Pending;
    }

    public static Payment Create(Guid orderId, decimal amount, PaymentMethod method, string provider)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order identifier cannot be empty.", nameof(orderId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Payment amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("Payment provider is required.", nameof(provider));

        return new Payment(
            Guid.NewGuid(),
            orderId,
            amount,
            method,
            provider.Trim());
    }

    public void MarkAsSucceeded(string providerPaymentId)
    {
        if (string.IsNullOrWhiteSpace(providerPaymentId))
            throw new ArgumentException("Provider payment identifier is required.", nameof(providerPaymentId));

        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be completed.");

        Status = PaymentStatus.Succeeded;
        ProviderPaymentId = providerPaymentId.Trim();
        FailureReason = null;

        RaiseDomainEvent(new PaymentSucceededDomainEvent(Id, OrderId, Amount));
    }

    public void MarkAsFailed(string failureReason)
    {
        if (string.IsNullOrWhiteSpace(failureReason))
            throw new ArgumentException("Failure reason is required.", nameof(failureReason));

        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be failed.");

        Status = PaymentStatus.Failed;
        FailureReason = failureReason.Trim();
    }
}