using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.DeliverOrder;

public sealed class DeliverOrderCommandValidator : AbstractValidator<DeliverOrderCommand>
{
    public DeliverOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.OrderIdRequiredCode)
            .WithMessage(OrderValidationErrors.OrderIdRequiredMessage);
    }
}