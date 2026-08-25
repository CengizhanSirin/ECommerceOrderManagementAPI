using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;

public sealed class GetCouponByIdQueryValidator : AbstractValidator<GetCouponByIdQuery>
{
    public GetCouponByIdQueryValidator()
    {
        RuleFor(query => query.CouponId)
         .NotEmpty()
         .WithErrorCode(CouponValidationErrors.CouponIdRequiredCode)
         .WithMessage(CouponValidationErrors.CouponIdRequiredMessage);
    }
}