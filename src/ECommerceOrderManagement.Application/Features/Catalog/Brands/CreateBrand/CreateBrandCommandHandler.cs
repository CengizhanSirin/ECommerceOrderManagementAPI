using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Catalog;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.CreateBrand;

public sealed class CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateBrandCommand, CreateBrandResponse>
{
    public async Task<Result<CreateBrandResponse>> Handle(CreateBrandCommand command, CancellationToken cancellationToken)
    {
        var nameExists = await brandRepository.ExistsByNameAsync(command.Name, cancellationToken: cancellationToken);


        if (nameExists)
        {
            return Result<CreateBrandResponse>.Failure(BrandErrors.NameAlreadyExists(command.Name));

        }

        var slugExists = await brandRepository.ExistsBySlugAsync(command.Slug, cancellationToken: cancellationToken);


        if (slugExists)
        {
            return Result<CreateBrandResponse>.Failure(BrandErrors.SlugAlreadyExists(command.Slug));

        }

        var brand = Brand.Create(
            command.Name,
            command.Slug,
            command.Description,
            command.LogoUrl);

        await brandRepository.AddAsync(brand, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);


        return Result<CreateBrandResponse>.Success(new CreateBrandResponse
        {
            Id = brand.Id
        });
    }
}