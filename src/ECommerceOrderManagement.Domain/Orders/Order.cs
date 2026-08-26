using ECommerceOrderManagement.Domain.Common;
using ECommerceOrderManagement.Domain.Coupons;

namespace ECommerceOrderManagement.Domain.Orders;

public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = [];

    public Guid CustomerId { get; private set; }

    public string OrderNumber { get; private set; } = string.Empty;

    public OrderStatus Status { get; private set; }

    public OrderAddress ShippingAddress { get; private set; } = null!;

    public OrderAddress BillingAddress { get; private set; } = null!;

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public string? CouponCode { get; private set; }

    public DiscountType? DiscountType { get; private set; }

    public decimal? DiscountValue { get; private set; }

    public decimal DiscountAmount { get; private set; }

    public decimal Subtotal { get; private set; }

    public decimal TotalAmount { get; private set; }

    public string? CancellationReason { get; private set; }

    private Order()
    {
    }

    private Order(Guid customerId, string orderNumber, OrderAddress shippingAddress, OrderAddress billingAddress)
    {
        CustomerId = customerId;
        OrderNumber = orderNumber.Trim();
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Status = OrderStatus.Pending;
    }

    public static Order Create(Guid customerId, string orderNumber, OrderAddress shippingAddress, OrderAddress billingAddress, IEnumerable<OrderItemSnapshot> itemSnapshots)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
        }

        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ArgumentException("Order number cannot be empty.", nameof(orderNumber));
        }

        ArgumentNullException.ThrowIfNull(shippingAddress);
        ArgumentNullException.ThrowIfNull(billingAddress);
        ArgumentNullException.ThrowIfNull(itemSnapshots);

        var snapshots = itemSnapshots.ToList();

        if (snapshots.Count == 0)
        {
            throw new InvalidOperationException("An order must contain at least one item.");
        }

        var duplicateProduct = snapshots.GroupBy(snapshot => snapshot.ProductId).FirstOrDefault(group => group.Count() > 1);

        if (duplicateProduct is not null)
        {
            throw new InvalidOperationException($"Product ID '{duplicateProduct.Key}' cannot appear more than once in an order.");
        }

        var order = new Order(customerId, orderNumber, shippingAddress, billingAddress);

        foreach (var snapshot in snapshots)
        {
            var orderItem = OrderItem.Create(
                order.Id,
                snapshot.ProductId,
                snapshot.ProductName,
                snapshot.Sku,
                snapshot.UnitPrice,
                snapshot.Quantity);

            order._items.Add(orderItem);
        }

        order.Subtotal = order._items.Sum(item => item.LineTotal);
        order.DiscountAmount = 0m;
        order.TotalAmount = order.Subtotal;

        return order;
    }

    public void MarkAsPaid()
    {
        EnsureStatus(OrderStatus.Pending, "Only pending orders can be marked as paid.");

        Status = OrderStatus.Paid;
    }

    public void StartProcessing()
    {
        EnsureStatus(OrderStatus.Paid, "Only paid orders can start processing.");

        Status = OrderStatus.Processing;
    }

    public void Ship()
    {
        EnsureStatus(OrderStatus.Processing, "Only processing orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        EnsureStatus(OrderStatus.Shipped, "Only shipped orders can be delivered.");

        Status = OrderStatus.Delivered;
    }

    public void Cancel(string reason)
    {
        EnsureStatus(OrderStatus.Pending, "Only pending orders can be cancelled.");

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Cancellation reason cannot be empty.", nameof(reason));
        }

        if (reason.Trim().Length > 500)
        {
            throw new ArgumentException("Cancellation reason cannot exceed 500 characters.", nameof(reason));
        }

        Status = OrderStatus.Cancelled;
        CancellationReason = reason.Trim();
    }

    private void EnsureStatus(OrderStatus expectedStatus, string errorMessage)
    {
        if (Status != expectedStatus)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public void ApplyDiscount(string couponCode, DiscountType discountType, decimal discountValue, decimal discountAmount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(couponCode);

        if (CouponCode is not null)
        {
            throw new InvalidOperationException("An order can only have one coupon.");
        }

        if (discountValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(discountValue), "Discount value must be greater than zero.");
        }

        if (discountAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(discountAmount), "Discount amount must be greater than zero.");
        }

        if (discountAmount >= Subtotal)
        {
            throw new InvalidOperationException("Discount amount must be less than order subtotal.");
        }

        CouponCode = couponCode.Trim().ToUpperInvariant();
        DiscountType = discountType;
        DiscountValue = discountValue;
        DiscountAmount = discountAmount;
        TotalAmount = Subtotal - DiscountAmount;
    }
}