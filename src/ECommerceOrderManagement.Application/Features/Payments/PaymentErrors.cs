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

    public static Error OrderNotPayable(Guid orderId)
    {
        return Error.Conflict("Payment.OrderNotPayable", $"Order with ID '{orderId}' is not in a payable status.");
    }
}