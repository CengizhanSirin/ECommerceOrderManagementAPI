using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;

public sealed record ProcessPaymentCommand(Guid OrderId, string CardHolderName, string CardNumber, string ExpireMonth,
    string ExpireYear, string Cvc) : ICommand<ProcessPaymentResponse>;