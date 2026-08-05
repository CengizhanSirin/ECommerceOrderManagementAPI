using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.ShipOrder;

public sealed class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(command => command.OrderId)
       .NotEmpty()
       .WithErrorCode(OrderValidationErrors.OrderIdRequiredCode)
       .WithMessage(OrderValidationErrors.OrderIdRequiredMessage);
    }
}