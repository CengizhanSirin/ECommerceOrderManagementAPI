using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;

public sealed class GetCategoriesQueryHandler(ICategoryQueries categoryQueries) : IQueryHandler<GetCategoriesQuery, IReadOnlyCollection<GetCategoriesItemResponse>>
{
    public async Task<Result<IReadOnlyCollection<GetCategoriesItemResponse>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await categoryQueries.GetAllAsync(query, cancellationToken);

        return Result<IReadOnlyCollection<GetCategoriesItemResponse>>.Success(categories);
    }
}