using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Domain.ValueObjects;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound(command.ProductId));
        }

        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        if (!category.IsActive)
        {
            return Result.Failure(CategoryErrors.Inactive(command.CategoryId));
        }

        if (command.BrandId.HasValue)
        {
            var brand = await brandRepository.GetByIdAsync(command.BrandId.Value, cancellationToken);

            if (brand is null)
            {
                return Result.Failure(BrandErrors.NotFound(command.BrandId.Value));
            }

            if (!brand.IsActive)
            {
                return Result.Failure(BrandErrors.Inactive(command.BrandId.Value));
            }
        }

        var skuAlreadyExists = await productRepository.ExistsBySkuAsync(command.Sku, command.ProductId, cancellationToken);

        if (skuAlreadyExists)
        {
            return Result.Failure(ProductErrors.SkuAlreadyExists(command.Sku));
        }

        var slugAlreadyExists = await productRepository.ExistsBySlugAsync(command.Slug, command.ProductId, cancellationToken);

        if (slugAlreadyExists)
        {
            return Result.Failure(ProductErrors.SlugAlreadyExists(command.Slug));
        }

        var price = Money.Create(command.PriceAmount, command.Currency);

        product.Update(
            command.Name,
            command.Slug,
            command.Sku,
            price,
            command.CategoryId,
            command.BrandId,
            command.Description,
            command.MainImageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}