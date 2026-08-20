using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Payments;

namespace ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;

internal sealed class ProcessPaymentCommandHandler(IPaymentRepository paymentRepository, IPaymentService paymentService, IUnitOfWork unitOfWork)
 : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResponse>
{
    public async Task<Result<ProcessPaymentResponse>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        var alreadyPaid = await paymentRepository.ExistsSuccessfulPaymentByOrderIdAsync(command.OrderId, cancellationToken);

        if (alreadyPaid)
        {
            return Result<ProcessPaymentResponse>.Failure(PaymentErrors.AlreadyPaid(command.OrderId));
        }

        var payment = Payment.Create(command.OrderId, command.Amount, PaymentMethod.CreditCard, paymentService.ProviderName);

        var paymentRequest = new PaymentRequest(
            command.OrderId,
            command.Amount,
            command.CardHolderName,
            command.CardNumber,
            command.ExpireMonth,
            command.ExpireYear,
            command.Cvc);

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

        await paymentRepository.AddAsync(payment, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProcessPaymentResponse>.Success(new ProcessPaymentResponse(payment.Id, payment.Status.ToString(), payment.ProviderPaymentId));

    }
}