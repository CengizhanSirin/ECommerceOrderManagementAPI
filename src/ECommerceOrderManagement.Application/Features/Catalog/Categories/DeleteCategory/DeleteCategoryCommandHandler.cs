using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        var hasProducts = await productRepository.ExistsByCategoryIdAsync(command.CategoryId, cancellationToken);

        if (hasProducts)
        {
            return Result.Failure(CategoryErrors.HasProducts(command.CategoryId));
        }

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        category.Delete(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}