using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderItemValidator : AbstractValidator<CreateOrderItem>
{
    public CreateOrderItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.ProductIdRequiredCode)
            .WithMessage(OrderValidationErrors.ProductIdRequiredMessage);

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithErrorCode(OrderValidationErrors.QuantityMustBePositiveCode)
            .WithMessage(OrderValidationErrors.QuantityMustBePositiveMessage);
    }
}