using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductQueries productQueries) : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productQueries.GetByIdAsync(request.ProductId, cancellationToken);


        if (product is null)
        {
            return Result<GetProductByIdResponse>.Failure(ProductErrors.NotFound(request.ProductId));
        }

        return Result<GetProductByIdResponse>.Success(product);
    }
}