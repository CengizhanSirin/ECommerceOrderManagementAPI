using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;
using ECommerceOrderManagement.Domain.Coupons;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.Coupons;

public sealed class CreateCouponCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCouponAndSaveChanges_WhenRequestIsValid()
    {
        // Arrange
        var couponRepositoryMock = new Mock<ICouponRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var startsAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var command = new CreateCouponCommand(
            Code: "DISCOUNT10",
            DiscountType: DiscountType.Percentage,
            DiscountValue: 10m,
            MinimumOrderAmount: 100m,
            StartsAtUtc: startsAtUtc,
            EndsAtUtc: startsAtUtc.AddDays(30),
            UsageLimit: 100,
            UsageLimitPerUser: 1);

        couponRepositoryMock
            .Setup(repository => repository.ExistsByCodeAsync(command.Code, CancellationToken.None))
            .ReturnsAsync(false);

        couponRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Coupon>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new CreateCouponCommandHandler(couponRepositoryMock.Object, unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);

        couponRepositoryMock.Verify(
            repository => repository.AddAsync(

                It.Is<Coupon>(coupon =>

                    coupon.Id == result.Value.Id && coupon.Code == command.Code &&
                    coupon.DiscountType == command.DiscountType && coupon.DiscountValue == command.DiscountValue),

                CancellationToken.None), Times.Once());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCouponCodeAlreadyExists()
    {
        // Arrange
        var couponRepositoryMock = new Mock<ICouponRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var startsAtUtc = new DateTime(
            2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var command = new CreateCouponCommand(
            Code: "DISCOUNT10",
            DiscountType: DiscountType.Percentage,
            DiscountValue: 10m,
            MinimumOrderAmount: 100m,
            StartsAtUtc: startsAtUtc,
            EndsAtUtc: startsAtUtc.AddDays(30),
            UsageLimit: 100,
            UsageLimitPerUser: 1);

        couponRepositoryMock
            .Setup(repository => repository.ExistsByCodeAsync(command.Code, CancellationToken.None))
            .ReturnsAsync(true);

        var handler = new CreateCouponCommandHandler(
            couponRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Coupon.CodeAlreadyExists", result.Error.Code);

        couponRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Coupon>(), CancellationToken.None), Times.Never());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}