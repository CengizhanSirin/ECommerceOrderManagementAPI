using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.UpdateCategory;

public sealed class UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        var nameExists = await categoryRepository.ExistsByNameAsync(
            command.Name,
            command.CategoryId,
            cancellationToken);

        if (nameExists)
        {
            return Result.Failure(CategoryErrors.NameAlreadyExists(command.Name));
        }

        var slugExists = await categoryRepository.ExistsBySlugAsync(
            command.Slug,
            command.CategoryId,
            cancellationToken);

        if (slugExists)
        {
            return Result.Failure(CategoryErrors.SlugAlreadyExists(command.Slug));
        }

        category.Update(
            command.Name,
            command.Slug,
            command.Description,
            command.ImageUrl,
            command.DisplayOrder);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}