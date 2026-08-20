namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public sealed record PaymentRequest(
    Guid OrderId,
    decimal Amount,
    string CardHolderName,
    string CardNumber,
    string ExpireMonth,
    string ExpireYear,
    string Cvc);