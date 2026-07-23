using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;

public sealed record CreateProductCommand(string Name, string Slug, string Sku, decimal PriceAmount, string Currency,
                                          Guid CategoryId, Guid? BrandId, string? Description, string? MainImageUrl) : ICommand<Guid>;