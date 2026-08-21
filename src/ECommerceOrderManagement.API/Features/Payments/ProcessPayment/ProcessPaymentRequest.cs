namespace ECommerceOrderManagement.API.Features.Payments.ProcessPayment;

public sealed record ProcessPaymentRequest(
    Guid OrderId,
    string CardHolderName,
    string CardNumber,
    string ExpireMonth,
    string ExpireYear,
    string Cvc);