using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Domain.Addresses;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(IProductQueries productQueries, IInventoryRepository inventoryRepository, IOrderRepository orderRepository,
    IOrderNumberGenerator orderNumberGenerator, IUnitOfWork unitOfWork, ICurrentUser currentUser, IAddressRepository addressRepository)
    : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var shippingAddressEntity = await addressRepository.GetByIdAndUserIdAsync(command.ShippingAddressId, currentUser.UserId, cancellationToken);

        if (shippingAddressEntity is null)
        {
            return Result<CreateOrderResponse>.Failure(AddressErrors.NotFound(command.ShippingAddressId));
        }

        var productIds = command.Items.Select(item => item.ProductId).ToArray();

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

        foreach (var requestedItem in command.Items)
        {
            if (!inventoryItemsByProductId.TryGetValue(requestedItem.ProductId, out var inventoryItem))
            {
                return Result<CreateOrderResponse>.Failure(OrderErrors.InventoryNotFound(requestedItem.ProductId));
            }

            if (inventoryItem.AvailableQuantity < requestedItem.Quantity)
            {
                return Result<CreateOrderResponse>.Failure(OrderErrors.InsufficientStock(requestedItem.ProductId));

            }
        }

        var orderNumber = orderNumberGenerator.Generate();

        if (await orderRepository.ExistsByOrderNumberAsync(orderNumber, cancellationToken))
        {
            return Result<CreateOrderResponse>.Failure(OrderErrors.OrderNumberConflict(orderNumber));
        }

        var shippingAddress = CreateOrderAddress(shippingAddressEntity);

        var billingAddress = CreateOrderAddress(command.BillingAddress);

        var itemSnapshots = command.Items.Select(requestedItem =>
            {
                var product = productsById[requestedItem.ProductId];

                return new OrderItemSnapshot(
                    product.ProductId,
                    product.Name,
                    product.Sku,
                    product.UnitPrice,
                    requestedItem.Quantity);
            })
            .ToArray();

        var order = Order.Create(
            currentUser.UserId,
            orderNumber,
            shippingAddress,
            billingAddress,
            itemSnapshots);

        foreach (var requestedItem in command.Items)
        {
            var inventoryItem = inventoryItemsByProductId[requestedItem.ProductId];

            inventoryItem.ReserveStock(requestedItem.Quantity, $"Reserved for order {orderNumber}.");
        }

        await orderRepository.AddAsync(order, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateOrderResponse>.Success(new CreateOrderResponse(order.Id, order.OrderNumber));
    }

    private static OrderAddress CreateOrderAddress(CreateOrderAddress address)
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