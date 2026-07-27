using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

public sealed class GetProductsQueryHandler(IProductQueries productQueries) : IQueryHandler<GetProductsQuery, PagedResult<GetProductsItemResponse>>
{
    public async Task<Result<PagedResult<GetProductsItemResponse>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productQueries.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<GetProductsItemResponse>>.Success(products);
    }
}