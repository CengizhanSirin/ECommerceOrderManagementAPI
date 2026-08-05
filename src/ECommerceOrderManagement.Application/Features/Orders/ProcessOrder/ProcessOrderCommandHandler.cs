using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Orders;

namespace ECommerceOrderManagement.Application.Features.Orders.ProcessOrder;

internal sealed class ProcessOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : ICommandHandler<ProcessOrderCommand>
{
    public async Task<Result> Handle(ProcessOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        if (order.Status != OrderStatus.Paid)
        {
            return Result.Failure(OrderErrors.CannotStartProcessing(order.Id));
        }

        order.StartProcessing();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}