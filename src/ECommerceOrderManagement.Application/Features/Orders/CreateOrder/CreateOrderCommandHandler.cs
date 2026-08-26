using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Coupons;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Domain.Coupons;
using ECommerceOrderManagement.Domain.Orders;
using System.Data;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(IProductQueries productQueries, IInventoryRepository inventoryRepository, IOrderRepository orderRepository,
    IOrderNumberGenerator orderNumberGenerator, IUnitOfWork unitOfWork, ICurrentUser currentUser,
    IAddressRepository addressRepository, IShoppingCartRepository shoppingCartRepository,
    ICouponQueries couponQueries, ICouponRepository couponRepository)
    : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var shippingAddressEntity = await addressRepository.GetByIdAndUserIdAsync(command.ShippingAddressId, currentUser.UserId, cancellationToken);

        if (shippingAddressEntity is null)
        {
            return Result<CreateOrderResponse>.Failure(AddressErrors.NotFound(command.ShippingAddressId));
        }

        var billingAddressEntity = await addressRepository.GetByIdAndUserIdAsync(command.BillingAddressId, currentUser.UserId, cancellationToken);

        if (billingAddressEntity is null)
        {
            return Result<CreateOrderResponse>.Failure(AddressErrors.NotFound(command.BillingAddressId));
        }

        var shoppingCart = await shoppingCartRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        if (shoppingCart is null || shoppingCart.Items.Count == 0)
        {
            return Result<CreateOrderResponse>.Failure(ShoppingCartErrors.Empty());
        }

        var productIds = shoppingCart.Items.Select(item => item.ProductId).ToArray();

        var products = await productQueries.GetActiveOrderSnapshotsByIdsAsync(productIds, cancellationToken);

        var productsById = products.ToDictionary(product => product.ProductId);

        foreach (var productId in productIds)
        {
            if (!productsById.ContainsKey(productId))
            {
                return Result<CreateOrderResponse>.Failure(OrderErrors.ProductNotFound(productId));
            }
        }

        var inventoryItems = await inventoryRepository.GetByProductIdsAsync(productIds, cancellationToken);

        var inventoryItemsByProductId = inventoryItems.ToDictionary(inventoryItem => inventoryItem.ProductId);

        foreach (var cartItem in shoppingCart.Items)
        {
            if (!inventoryItemsByProductId.TryGetValue(cartItem.ProductId, out var inventoryItem))
            {
                return Result<CreateOrderResponse>.Failure(OrderErrors.InventoryNotFound(cartItem.ProductId));
            }

            if (inventoryItem.AvailableQuantity < cartItem.Quantity)
            {
                return Result<CreateOrderResponse>.Failure(OrderErrors.InsufficientStock(cartItem.ProductId));

            }
        }

        var orderNumber = orderNumberGenerator.Generate();

        if (await orderRepository.ExistsByOrderNumberAsync(orderNumber, cancellationToken))
        {
            return Result<CreateOrderResponse>.Failure(OrderErrors.OrderNumberConflict(orderNumber));
        }

        var shippingAddress = CreateOrderAddress(shippingAddressEntity);

        var billingAddress = CreateOrderAddress(billingAddressEntity);

        var itemSnapshots = shoppingCart.Items.Select(cartItem =>
            {
                var product = productsById[cartItem.ProductId];

                return new OrderItemSnapshot(
                    product.ProductId,
                    product.Name,
                    product.Sku,
                    product.UnitPrice,
                    cartItem.Quantity);
            })
            .ToArray();

        var order = Order.Create(
            currentUser.UserId,
            orderNumber,
            shippingAddress,
            billingAddress,
            itemSnapshots);

        var couponTransactionStarted = false;

        async Task RollbackCouponTransactionAsync()
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            couponTransactionStarted = false;
        }

        CouponEvaluationReadModel? coupon = null;

        try
        {

            if (!string.IsNullOrWhiteSpace(command.CouponCode))
            {
                await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

                couponTransactionStarted = true;

                var normalizedCouponCode = command.CouponCode.Trim().ToUpperInvariant();

                await couponRepository.AcquireUsageLockAsync( normalizedCouponCode, cancellationToken);

                coupon = await couponQueries.GetForEvaluationAsync(normalizedCouponCode, currentUser.UserId, cancellationToken);

                if (coupon is null)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.NotFoundByCode(normalizedCouponCode));
                }

                if (!coupon.IsActive)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.Inactive(normalizedCouponCode));
                }

                var utcNow = DateTime.UtcNow;

                if (utcNow < coupon.StartsAtUtc)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.NotStarted(normalizedCouponCode));
                }

                if (utcNow >= coupon.EndsAtUtc)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.Expired(normalizedCouponCode));
                }

                if (order.Subtotal < coupon.MinimumOrderAmount)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.MinimumOrderAmountNotMet(normalizedCouponCode, coupon.MinimumOrderAmount));
                }

                if (coupon.TotalUsageCount >= coupon.UsageLimit)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.UsageLimitReached(normalizedCouponCode));
                }

                if (coupon.UserUsageCount >= coupon.UsageLimitPerUser)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.UserUsageLimitReached(normalizedCouponCode));
                }

                decimal discountAmount = coupon.DiscountType switch
                {
                    DiscountType.Percentage => decimal.Round(order.Subtotal * coupon.DiscountValue / 100m, 2, MidpointRounding.AwayFromZero),

                    DiscountType.FixedAmount => coupon.DiscountValue,

                    _ => throw new InvalidOperationException($"Unsupported discount type '{coupon.DiscountType}'.")
                };

                if (coupon.DiscountType == DiscountType.FixedAmount && discountAmount > order.Subtotal)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.DiscountExceedsSubtotal());
                }

                if (discountAmount >= order.Subtotal)
                {
                    await RollbackCouponTransactionAsync();

                    return Result<CreateOrderResponse>.Failure(CouponErrors.DiscountWouldMakeOrderFree());
                }

                order.ApplyDiscount(coupon.Code, coupon.DiscountType, coupon.DiscountValue, discountAmount);

                var couponUsage = CouponUsage.Create(coupon.Id, currentUser.UserId, order.Id);

                await couponRepository.AddUsageAsync(couponUsage, cancellationToken);
            }


            foreach (var cartItem in shoppingCart.Items)
            {
                var inventoryItem = inventoryItemsByProductId[cartItem.ProductId];

                inventoryItem.ReserveStock(cartItem.Quantity, $"Reserved for order {orderNumber}.");
            }

            await orderRepository.AddAsync(order, cancellationToken);

            shoppingCart.Clear();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (couponTransactionStarted)
            {
                await unitOfWork.CommitTransactionAsync(cancellationToken);

                couponTransactionStarted = false;
            }


            return Result<CreateOrderResponse>.Success(new CreateOrderResponse(order.Id, order.OrderNumber));

        }
        catch (Exception)
        {
            if (couponTransactionStarted)
            {
                await RollbackCouponTransactionAsync();
            }
            throw;
        }
    }

    private static OrderAddress CreateOrderAddress(Address address)
    {
        return OrderAddress.Create(
            address.FullName,
            address.PhoneNumber,
            address.Country,
            address.City,
            address.District,
            address.PostalCode,
            address.AddressLine);
    }
}