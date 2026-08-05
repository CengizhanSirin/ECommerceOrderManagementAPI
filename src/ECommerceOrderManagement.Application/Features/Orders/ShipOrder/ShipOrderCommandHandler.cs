using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.ShipOrder;

internal sealed class ShipOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : ICommandHandler<ShipOrderCommand>
{
    public async Task<Result> Handle(ShipOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        if (order.Status != OrderStatus.Processing)
        {
            return Result.Failure(OrderErrors.CannotShip(order.Id));
        }

        order.Ship();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}