using ECommerceOrderManagement.Application.Features.Coupons;
using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.ShippingAddressId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.ShippingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.ShippingAddressRequiredMessage);

        RuleFor(command => command.BillingAddressId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.BillingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.BillingAddressRequiredMessage);

        RuleFor(command => command.CouponCode)
            .MaximumLength(50)
            .When(command => !string.IsNullOrWhiteSpace(command.CouponCode))
            .WithErrorCode(CouponValidationErrors.CodeMaxLengthCode)
            .WithMessage(CouponValidationErrors.CodeMaxLengthMessage);
    }
}