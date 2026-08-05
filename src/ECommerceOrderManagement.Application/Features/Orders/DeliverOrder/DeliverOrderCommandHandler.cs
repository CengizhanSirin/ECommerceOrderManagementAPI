using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.DeliverOrder;

internal sealed class DeliverOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeliverOrderCommand>
{
    public async Task<Result> Handle(DeliverOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        if (order.Status != OrderStatus.Shipped)
        {
            return Result.Failure(OrderErrors.CannotDeliver(order.Id));
        }

        order.Deliver();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}