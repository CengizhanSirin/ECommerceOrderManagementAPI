using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Coupons.ActivateCoupon;

public sealed class ActivateCouponCommandValidator : AbstractValidator<ActivateCouponCommand>
{
    public ActivateCouponCommandValidator()
    {
        RuleFor(command => command.CouponId)
            .NotEmpty()
            .WithErrorCode(CouponValidationErrors.CouponIdRequiredCode)
            .WithMessage(CouponValidationErrors.CouponIdRequiredMessage);
    }
}