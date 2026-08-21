using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Domain.Orders;
using ECommerceOrderManagement.Domain.Payments;

namespace ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;

internal sealed class ProcessPaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentService paymentService, IUnitOfWork unitOfWork,
    IOrderRepository orderRepository, ICurrentUser currentUser)
 : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResponse>
{
    public async Task<Result<ProcessPaymentResponse>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAndCustomerIdWithItemsAsync(command.OrderId, currentUser.UserId, cancellationToken);

        if (order is null)
        {
            return Result<ProcessPaymentResponse>.Failure(OrderErrors.NotFound(command.OrderId));
        }

        var alreadyPaid = await paymentRepository.ExistsSuccessfulPaymentByOrderIdAsync(command.OrderId, cancellationToken);

        if (alreadyPaid)
        {
            return Result<ProcessPaymentResponse>.Failure(PaymentErrors.AlreadyPaid(command.OrderId));
        }

        if (order.Status != OrderStatus.Pending)
        {
            return Result<ProcessPaymentResponse>.Failure(PaymentErrors.OrderNotPayable(command.OrderId));
        }

        var payment = Payment.Create(command.OrderId, order.TotalAmount, PaymentMethod.CreditCard, paymentService.ProviderName);

        var shippingAddress = new PaymentAddress(
            order.ShippingAddress.FullName,
            order.ShippingAddress.PhoneNumber,
            order.ShippingAddress.Country,
            order.ShippingAddress.City,
            order.ShippingAddress.District,
            order.ShippingAddress.PostalCode ?? string.Empty,
            order.ShippingAddress.AddressLine);

        var billingAddress = new PaymentAddress(
            order.BillingAddress.FullName,
            order.BillingAddress.PhoneNumber,
            order.BillingAddress.Country,
            order.BillingAddress.City,
            order.BillingAddress.District,
            order.BillingAddress.PostalCode ?? string.Empty,
            order.BillingAddress.AddressLine);

        var paymentItems = order.Items
            .Select(item => new PaymentItem(
                item.ProductId,
                item.ProductName,
                item.Sku,
                item.UnitPrice,
                item.Quantity,
                item.LineTotal))
            .ToArray();

        var paymentRequest = new PaymentRequest(
            order.Id,
            order.CustomerId,
            order.TotalAmount,
            command.CardHolderName,
            command.CardNumber,
            command.ExpireMonth,
            command.ExpireYear,
            command.Cvc,
            shippingAddress,
            billingAddress,
            paymentItems);

        var paymentResult = await paymentService.ProcessPaymentAsync(paymentRequest, cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            var failureReason = paymentResult.FailureReason ?? "Payment was declined.";

            payment.MarkAsFailed(failureReason);

            await paymentRepository.AddAsync(payment, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ProcessPaymentResponse>.Failure(PaymentErrors.Failed(failureReason));
        }

        payment.MarkAsSucceeded(paymentResult.ProviderPaymentId!);

        order.MarkAsPaid();

        await paymentRepository.AddAsync(payment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProcessPaymentResponse>.Success(new ProcessPaymentResponse(payment.Id, payment.Status.ToString(), payment.ProviderPaymentId));

    }
}