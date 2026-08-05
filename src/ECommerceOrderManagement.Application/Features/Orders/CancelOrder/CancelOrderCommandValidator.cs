using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CancelOrder;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.OrderIdRequiredCode)
            .WithMessage(OrderValidationErrors.OrderIdRequiredMessage);

        RuleFor(command => command.Reason)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.CancellationReasonRequiredCode)
            .WithMessage(OrderValidationErrors.CancellationReasonRequiredMessage)
            .MaximumLength(500)
            .WithErrorCode(OrderValidationErrors.CancellationReasonTooLongCode)
            .WithMessage(OrderValidationErrors.CancellationReasonTooLongMessage);
    }
}