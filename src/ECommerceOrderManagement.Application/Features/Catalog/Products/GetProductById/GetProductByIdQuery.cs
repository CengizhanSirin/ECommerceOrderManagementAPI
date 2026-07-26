using ECommerceOrderManagement.Application.Common.Messaging;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResponse>;