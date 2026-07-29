using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Products;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.DeleteBrand;

public sealed class DeleteBrandCommandHandler(IBrandRepository brandRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, TimeProvider timeProvider)
    : ICommandHandler<DeleteBrandCommand>
{
    public async Task<Result> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetByIdAsync(command.BrandId, cancellationToken);

        if (brand is null)
        {
            return Result.Failure(BrandErrors.NotFound(command.BrandId));
        }

        var hasProducts = await productRepository.ExistsByBrandIdAsync(command.BrandId, cancellationToken);

        if (hasProducts)
        {
            return Result.Failure(BrandErrors.HasProducts(command.BrandId));
        }

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        brand.Delete(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}