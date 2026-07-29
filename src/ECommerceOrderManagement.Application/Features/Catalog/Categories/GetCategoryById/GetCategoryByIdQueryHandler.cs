using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(ICategoryQueries categoryQueries) : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    public async Task<Result<GetCategoryByIdResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryQueries.GetByIdAsync(query.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result<GetCategoryByIdResponse>.Failure(CategoryErrors.NotFound(query.CategoryId));
        }

        return Result<GetCategoryByIdResponse>.Success(category);
    }
}