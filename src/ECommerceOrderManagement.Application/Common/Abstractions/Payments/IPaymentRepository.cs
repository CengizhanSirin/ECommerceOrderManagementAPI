using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Domain.Payments;

namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<bool> ExistsSuccessfulPaymentByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}