using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Persistence.Repositories.Orders.Queries;

internal sealed class OrderQueries(ApplicationDbContext dbContext) : IOrderQueries
{
    public Task<GetOrderByIdResponse?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return dbContext.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new GetOrderByIdResponse(
                order.Id,
                order.CustomerId,
                order.OrderNumber,
                order.Status,
                order.Items.Sum(item => item.UnitPrice * item.Quantity),
                order.Items.Sum(item => item.UnitPrice * item.Quantity),
                order.CancellationReason,
                new GetOrderByIdAddressResponse(
                          order.ShippingAddress.FullName,
                          order.ShippingAddress.PhoneNumber,
                          order.ShippingAddress.Country,
                          order.ShippingAddress.City,
                          order.ShippingAddress.District,
                          order.ShippingAddress.PostalCode,
                          order.ShippingAddress.AddressLine),
                new GetOrderByIdAddressResponse(
                          order.BillingAddress.FullName,
                          order.BillingAddress.PhoneNumber,
                          order.BillingAddress.Country,
                          order.BillingAddress.City,
                          order.BillingAddress.District,
                          order.BillingAddress.PostalCode,
                          order.BillingAddress.AddressLine),
                order.Items
             .OrderBy(item => item.CreatedAtUtc)
             .Select(item => new GetOrderByIdItemResponse(
                          item.Id,
                          item.ProductId,
                          item.ProductName,
                          item.Sku,
                          item.UnitPrice,
                          item.Quantity,
                          item.UnitPrice * item.Quantity))
             .ToList(),
                order.CreatedAtUtc,
                order.UpdatedAtUtc))
            .SingleOrDefaultAsync(cancellationToken);
    }
}