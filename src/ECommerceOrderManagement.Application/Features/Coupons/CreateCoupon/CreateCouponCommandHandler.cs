using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;

internal class CreateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateCouponCommand, CreateCouponResponse>
{
    public async Task<Result<CreateCouponResponse>> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var normalizedCode = command.Code.Trim().ToUpperInvariant();

        var codeExists = await couponRepository.ExistsByCodeAsync(normalizedCode, cancellationToken);


        if (codeExists)
        {
            return Result<CreateCouponResponse>.Failure(CouponErrors.CodeAlreadyExists(normalizedCode));
        }


        var coupon = Coupon.Create(
          normalizedCode,
          command.DiscountType,
          command.DiscountValue,
          command.MinimumOrderAmount,
          command.StartsAtUtc,
          command.EndsAtUtc,
          command.UsageLimit,
          command.UsageLimitPerUser);

        await couponRepository.AddAsync(coupon, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateCouponResponse>.Success(new CreateCouponResponse(coupon.Id));
    }
}