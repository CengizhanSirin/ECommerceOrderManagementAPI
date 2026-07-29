using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.UpdateBrand;

public sealed class UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateBrandCommand>
{
    public async Task<Result> Handle(UpdateBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(command.BrandId, cancellationToken);

        if (brand is null)
        {
            return Result.Failure(BrandErrors.NotFound(command.BrandId));
        }

        var nameExists = await brandRepository.ExistsByNameAsync(command.Name, command.BrandId, cancellationToken);

        if (nameExists)
        {
            return Result.Failure(BrandErrors.NameAlreadyExists(command.Name));
        }

        var slugExists = await brandRepository.ExistsBySlugAsync(command.Slug, command.BrandId, cancellationToken);

        if (slugExists)
        {
            return Result.Failure(BrandErrors.SlugAlreadyExists(command.Slug));
        }

        brand.Update(
            command.Name,
            command.Slug,
            command.Description,
            command.LogoUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}