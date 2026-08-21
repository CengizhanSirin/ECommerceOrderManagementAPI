namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public sealed record PaymentRequest(
    Guid OrderId,
    Guid CustomerId,
    decimal Amount,
    string CardHolderName,
    string CardNumber,
    string ExpireMonth,
    string ExpireYear,
    string Cvc,
    PaymentAddress ShippingAddress,
    PaymentAddress BillingAddress,
    IReadOnlyCollection<PaymentItem> Items);