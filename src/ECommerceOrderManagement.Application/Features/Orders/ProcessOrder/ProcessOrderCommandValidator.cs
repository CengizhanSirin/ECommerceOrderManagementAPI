using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.ProcessOrder;

public sealed class ProcessOrderCommandValidator : AbstractValidator<ProcessOrderCommand>
{
    public ProcessOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.OrderIdRequiredCode)
            .WithMessage(OrderValidationErrors.OrderIdRequiredMessage);
    }
}