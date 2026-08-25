using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Coupons.DeactivateCoupon;

internal sealed class DeactivateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeactivateCouponCommand>
{
    public async Task<Result> Handle(DeactivateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await couponRepository.GetByIdAsync(command.CouponId, cancellationToken);

        if (coupon is null)
        {
            return Result.Failure(CouponErrors.NotFound(command.CouponId));
        }

        coupon.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}