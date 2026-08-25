using ECommerceOrderManagement.Domain.Coupons;
using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;

public sealed class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty()
            .WithErrorCode(CouponValidationErrors.CodeRequiredCode)
            .WithMessage(CouponValidationErrors.CodeRequiredMessage)

            .MaximumLength(50)
            .WithErrorCode(CouponValidationErrors.CodeMaxLengthCode)
            .WithMessage(CouponValidationErrors.CodeMaxLengthMessage);

        RuleFor(command => command.DiscountType)
            .IsInEnum()
            .WithErrorCode(CouponValidationErrors.DiscountTypeInvalidCode)
            .WithMessage(CouponValidationErrors.DiscountTypeInvalidMessage);

        RuleFor(command => command.DiscountValue)
            .GreaterThan(0)
            .WithErrorCode(CouponValidationErrors.DiscountValuePositiveCode)
            .WithMessage(CouponValidationErrors.DiscountValuePositiveMessage);

        RuleFor(command => command.DiscountValue)
            .LessThanOrEqualTo(100)
            .When(command => command.DiscountType == DiscountType.Percentage)
            .WithErrorCode(CouponValidationErrors.PercentageValueRangeCode)
            .WithMessage(CouponValidationErrors.PercentageValueRangeMessage);

        RuleFor(command => command.MinimumOrderAmount)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(CouponValidationErrors.MinimumOrderAmountNonNegativeCode)
            .WithMessage(CouponValidationErrors.MinimumOrderAmountNonNegativeMessage);

        RuleFor(command => command.StartsAtUtc)
            .NotEmpty()
            .WithErrorCode(CouponValidationErrors.StartsAtRequiredCode)
            .WithMessage(CouponValidationErrors.StartsAtRequiredMessage);

        RuleFor(command => command.EndsAtUtc)
            .NotEmpty()
            .WithErrorCode(CouponValidationErrors.EndsAtRequiredCode)
            .WithMessage(CouponValidationErrors.EndsAtRequiredMessage);

        RuleFor(command => command.EndsAtUtc)
            .GreaterThan(command => command.StartsAtUtc)
            .WithErrorCode(CouponValidationErrors.DateRangeInvalidCode)
            .WithMessage(CouponValidationErrors.DateRangeInvalidMessage);

        RuleFor(command => command.UsageLimit)
            .GreaterThan(0)
            .WithErrorCode(CouponValidationErrors.UsageLimitPositiveCode)
            .WithMessage(CouponValidationErrors.UsageLimitPositiveMessage);

        RuleFor(command => command.UsageLimitPerUser)
            .GreaterThan(0)
            .WithErrorCode(CouponValidationErrors.UsageLimitPerUserPositiveCode)
            .WithMessage(CouponValidationErrors.UsageLimitPerUserPositiveMessage);

        RuleFor(command => command.UsageLimitPerUser)
            .LessThanOrEqualTo(command => command.UsageLimit)
            .WithErrorCode(CouponValidationErrors.UsageLimitPerUserExceededCode)
            .WithMessage(CouponValidationErrors.UsageLimitPerUserExceededMessage);
    }
}