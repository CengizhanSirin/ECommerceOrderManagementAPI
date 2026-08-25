using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Coupons.ActivateCoupon;

internal sealed class ActivateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork) : ICommandHandler<ActivateCouponCommand>
{
    public async Task<Result> Handle(ActivateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await couponRepository.GetByIdAsync(command.CouponId, cancellationToken);

        if (coupon is null)
        {
            return Result.Failure(CouponErrors.NotFound(command.CouponId));
        }

        coupon.Activate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}