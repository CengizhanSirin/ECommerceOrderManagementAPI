using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Coupons.DeactivateCoupon;

public sealed class DeactivateCouponCommandValidator:AbstractValidator<DeactivateCouponCommand>
{
    public DeactivateCouponCommandValidator()
    {
        RuleFor(command => command.CouponId)
        .NotEmpty()
        .WithErrorCode(CouponValidationErrors.CouponIdRequiredCode)
        .WithMessage(CouponValidationErrors.CouponIdRequiredMessage);
    }
}