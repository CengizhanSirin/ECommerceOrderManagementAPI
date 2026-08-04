namespace ECommerceOrderManagement.Application.Features.Catalog.Products;

public sealed record ProductOrderSnapshotDto(Guid ProductId, string Name, string Sku, decimal UnitPrice);