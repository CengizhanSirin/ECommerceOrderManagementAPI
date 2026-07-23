using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Domain.Catalog;
using ECommerceOrderManagement.Domain.ValueObjects;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result<Guid>.Failure(CategoryErrors.NotFound(command.CategoryId));
        }

        if (!category.IsActive)
        {
            return Result<Guid>.Failure(CategoryErrors.Inactive(command.CategoryId));
        }

        if (command.BrandId.HasValue)
        {
            var brand = await _brandRepository.GetByIdAsync(command.BrandId.Value, cancellationToken);

            if (brand is null)
            {
                return Result<Guid>.Failure(BrandErrors.NotFound(command.BrandId.Value));
            }

            if (!brand.IsActive)
            {
                return Result<Guid>.Failure(BrandErrors.Inactive(command.BrandId.Value));
            }
        }

        var skuAlreadyExists = await _productRepository.ExistsBySkuAsync(command.Sku, cancellationToken: cancellationToken);


        if (skuAlreadyExists)
        {
            return Result<Guid>.Failure(ProductErrors.SkuAlreadyExists(command.Sku));

        }

        var slugAlreadyExists = await _productRepository.ExistsBySlugAsync(command.Slug, cancellationToken: cancellationToken);


        if (slugAlreadyExists)
        {
            return Result<Guid>.Failure(ProductErrors.SlugAlreadyExists(command.Slug));
        }

        var price = Money.Create(command.PriceAmount, command.Currency);

        var product = Product.Create(command.Name, command.Slug, command.Sku, price, command.CategoryId, command.BrandId, command.Description, command.MainImageUrl);


        await _productRepository.AddAsync(product, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(product.Id);
    }
}