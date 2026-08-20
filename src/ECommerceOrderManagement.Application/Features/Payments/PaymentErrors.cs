using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Payments;

public static class PaymentErrors
{
    public static Error AlreadyPaid(Guid orderId)
    {
        return Error.Conflict("Payment.AlreadyPaid", $"Order with ID '{orderId}' already has a successful payment.");
    }

    public static Error Failed(string reason)
    {
        return Error.Failure("Payment.Failed", reason);
    }
}