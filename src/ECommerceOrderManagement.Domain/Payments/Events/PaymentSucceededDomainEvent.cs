using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Payments.Events;

public sealed record PaymentSucceededDomainEvent(Guid PaymentId, Guid OrderId, decimal Amount) : DomainEvent;