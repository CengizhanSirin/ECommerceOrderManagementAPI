using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Orders;

public sealed class OrderItem : AuditableEntity
{
    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public string Sku { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal LineTotal => UnitPrice * Quantity;

    private OrderItem()
    {
    }

    private OrderItem(Guid orderId, Guid productId, string productName, string sku, decimal unitPrice, int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName.Trim();
        Sku = sku.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal static OrderItem Create(Guid orderId, Guid productId, string productName, string sku, decimal unitPrice, int quantity)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(productName));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU cannot be empty.", nameof(sku));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        return new OrderItem(orderId, productId, productName, sku, unitPrice, quantity);
    }
}