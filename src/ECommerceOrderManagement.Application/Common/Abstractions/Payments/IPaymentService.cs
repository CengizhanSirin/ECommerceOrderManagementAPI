namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public interface IPaymentService
{
    string ProviderName { get; }

    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
}