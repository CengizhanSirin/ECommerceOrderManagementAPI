using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Orders;
using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;
using ECommerceOrderManagement.Application.Features.Orders.GetOrders;
using ECommerceOrderManagement.Domain.Orders;
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
                order.Subtotal,
                order.DiscountAmount,
                order.TotalAmount,
                order.CouponCode,
                order.DiscountType,
                order.DiscountValue,
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

    public async Task<PagedResult<GetOrdersItemResponse>> GetPagedAsync(GetOrdersQuery query, Guid customerId, CancellationToken cancellationToken = default)
    {
        IQueryable<Order> ordersQuery = dbContext.Orders.AsNoTracking().Where(order => order.CustomerId == customerId);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            ordersQuery = ordersQuery.Where(order => order.OrderNumber.Contains(searchTerm));
        }

        if (query.Status.HasValue)
        {
            ordersQuery = ordersQuery.Where(order => order.Status == query.Status.Value);
        }

        var totalCount = await ordersQuery.CountAsync(cancellationToken);

        var orderedQuery = ApplySorting(ordersQuery, query.SortBy, query.SortDirection);

        var items = await orderedQuery
            .ThenBy(order => order.Id)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(order => new GetOrdersItemResponse(
                order.Id,
                order.CustomerId,
                order.OrderNumber,
                order.Status,
                order.Items.Count(),
                order.TotalAmount,
                order.CreatedAtUtc,
                order.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<GetOrdersItemResponse>(items, query.PageNumber, query.PageSize, totalCount);
    }

    private static IOrderedQueryable<Order> ApplySorting(IQueryable<Order> query, string? sortBy, string? sortDirection)
    {
        var normalizedSortBy = string.IsNullOrWhiteSpace(sortBy)
            ? "createdatutc"
            : sortBy.Trim().ToLowerInvariant();

        var isDescending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return (normalizedSortBy, isDescending) switch
        {
            ("ordernumber", false) => query.OrderBy(order => order.OrderNumber),

            ("ordernumber", true) => query.OrderByDescending(order => order.OrderNumber),

            ("status", false) => query.OrderBy(order => order.Status),

            ("status", true) => query.OrderByDescending(order => order.Status),

            ("itemcount", false) => query.OrderBy(order => order.Items.Count()),

            ("itemcount", true) => query.OrderByDescending(order => order.Items.Count()),

            ("totalamount", false) => query.OrderBy(order => order.TotalAmount),

            ("totalamount", true) => query.OrderByDescending(order => order.TotalAmount),

            ("createdatutc", false) => query.OrderBy(order => order.CreatedAtUtc),

            _ => query.OrderByDescending(order => order.CreatedAtUtc)
        };
    }
}