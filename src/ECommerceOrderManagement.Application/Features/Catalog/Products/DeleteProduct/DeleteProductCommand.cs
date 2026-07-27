using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : ICommand
{
}