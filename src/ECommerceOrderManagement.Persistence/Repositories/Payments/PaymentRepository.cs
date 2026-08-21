using ECommerceOrderManagement.Application.Common.Abstractions.Payments;
using ECommerceOrderManagement.Domain.Payments;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Payments;

internal sealed class PaymentRepository(ApplicationDbContext dbContext) : Repository<Payment>(dbContext), IPaymentRepository
{
    public Task<bool> ExistsSuccessfulPaymentByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(payment => payment.OrderId == orderId && payment.Status == PaymentStatus.Succeeded, cancellationToken);
    }
}