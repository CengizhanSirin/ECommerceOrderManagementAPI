using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Inventory;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.CancelOrder;

internal sealed class CancelOrderCommandHandler(IOrderRepository orderRepository, IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelOrderCommand>
{
    public async Task<Result> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        if (order.Status != OrderStatus.Pending)
        {
            return Result.Failure(OrderErrors.CannotCancel(order.Id));
        }

        var productIds = order.Items.Select(item => item.ProductId).ToArray();

        var inventoryItems = await inventoryRepository.GetByProductIdsAsync(productIds, cancellationToken);

        var inventoryByProductId = inventoryItems.ToDictionary(item => item.ProductId);

        foreach (var orderItem in order.Items)
        {
            if (!inventoryByProductId.TryGetValue(orderItem.ProductId, out var inventoryItem))
            {
                return Result.Failure(OrderErrors.InventoryNotFound(orderItem.ProductId));
            }

            if (inventoryItem.ReservedQuantity < orderItem.Quantity)
            {
                return Result.Failure(OrderErrors.ReservationConflict(orderItem.ProductId));
            }
        }

        foreach (var orderItem in order.Items)
        {
            var inventoryItem = inventoryByProductId[orderItem.ProductId];

            inventoryItem.ReleaseStock(orderItem.Quantity, $"Released for cancelled order {order.OrderNumber}.");
        }

        order.Cancel(command.Reason);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}