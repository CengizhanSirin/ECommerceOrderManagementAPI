namespace ECommerceOrderManagement.Application.Common.Abstractions.Payments;

public sealed record PaymentItem(Guid ProductId, string ProductName, string Sku, decimal UnitPrice, int Quantity, decimal LineTotal);