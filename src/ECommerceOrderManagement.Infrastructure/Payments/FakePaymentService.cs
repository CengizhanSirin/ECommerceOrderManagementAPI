using ECommerceOrderManagement.Application.Common.Abstractions.Payments;

namespace ECommerceOrderManagement.Infrastructure.Payments;

internal sealed class FakePaymentService : IPaymentService
{
    public string ProviderName => "FakePayment";

    public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        if (request.CardNumber.EndsWith("0000"))
        {
            return Task.FromResult(new PaymentResult(false, null, "Payment was rejected by the fake payment provider."));
        }

        var providerPaymentId = $"FAKE-{Guid.NewGuid():N}";

        return Task.FromResult(new PaymentResult(true, providerPaymentId, null));
    }
}