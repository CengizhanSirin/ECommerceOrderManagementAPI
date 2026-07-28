using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Catalog;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.CreateCategory;

public sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var nameExists = await categoryRepository.ExistsByNameAsync(command.Name, cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Result<CreateCategoryResponse>.Failure(CategoryErrors.NameAlreadyExists(command.Name));
        }

        var slugExists = await categoryRepository.ExistsBySlugAsync(command.Slug, cancellationToken: cancellationToken);

        if (slugExists)
        {
            return Result<CreateCategoryResponse>.Failure(CategoryErrors.SlugAlreadyExists(command.Slug));
        }

        var category = Category.Create(
            command.Name,
            command.Slug,
            command.Description,
            command.ImageUrl,
            command.DisplayOrder);

        await categoryRepository.AddAsync(category, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);


        return Result<CreateCategoryResponse>.Success(new CreateCategoryResponse
        {
            Id = category.Id
        });
    }
}