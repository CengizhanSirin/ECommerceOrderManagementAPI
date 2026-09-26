using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Features.Catalog.Brands;
using ECommerceOrderManagement.Application.Features.Catalog.Categories;
using ECommerceOrderManagement.Application.Features.Catalog.Products;
using ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;
using ECommerceOrderManagement.Domain.Catalog;
using Moq;

namespace ECommerceOrderManagement.UnitTests.Application.Catalog;

public sealed class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateProductAndSaveChanges_WhenRequestIsValid()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var brandRepositoryMock = new Mock<IBrandRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var category = Category.Create("Electronics", "electronics");

        var command = new CreateProductCommand(
            Name: "Test Product",
            Slug: "test-product",
            Sku: "TEST-001",
            PriceAmount: 100m,
            Currency: "TRY",
            CategoryId: category.Id,
            BrandId: null,
            Description: null,
            MainImageUrl: null);

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(category.Id, CancellationToken.None))
            .ReturnsAsync(category);

        productRepositoryMock
            .Setup(repository => repository.ExistsBySkuAsync(command.Sku, null, CancellationToken.None))
            .ReturnsAsync(false);

        productRepositoryMock
            .Setup(repository => repository.ExistsBySlugAsync(command.Slug, null, CancellationToken.None))
            .ReturnsAsync(false);

        productRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Product>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1);

        var handler = new CreateProductCommandHandler(
            productRepositoryMock.Object,
            categoryRepositoryMock.Object,
            brandRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        productRepositoryMock.Verify(repository => repository.AddAsync(
                It.Is<Product>(product =>
                    product.Id == result.Value && product.Sku == command.Sku), CancellationToken.None),
            Times.Once());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSkuAlreadyExists()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var brandRepositoryMock = new Mock<IBrandRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var category = Category.Create("Electronics", "electronics");

        var command = new CreateProductCommand(
            Name: "Test Product",
            Slug: "test-product",
            Sku: "TEST-001",
            PriceAmount: 100m,
            Currency: "TRY",
            CategoryId: category.Id,
            BrandId: null,
            Description: null,
            MainImageUrl: null);

        categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(category.Id, CancellationToken.None))
            .ReturnsAsync(category);

        productRepositoryMock
            .Setup(repository => repository.ExistsBySkuAsync(command.Sku, null, CancellationToken.None))
            .ReturnsAsync(true);

        var handler = new CreateProductCommandHandler(
            productRepositoryMock.Object,
            categoryRepositoryMock.Object,
            brandRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Product.SkuAlreadyExists", result.Error.Code);

        productRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Product>(), CancellationToken.None), Times.Never());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryIsInactive()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var brandRepositoryMock = new Mock<IBrandRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var category = Category.Create("Electronics", "electronics");
        category.Deactivate();

        var command = new CreateProductCommand(
            Name: "Test Product",
            Slug: "test-product",
            Sku: "TEST-001",
            PriceAmount: 100m,
            Currency: "TRY",
            CategoryId: category.Id,
            BrandId: null,
            Description: null,
            MainImageUrl: null);

        categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(category.Id, CancellationToken.None)).ReturnsAsync(category);

        var handler = new CreateProductCommandHandler(
            productRepositoryMock.Object,
            categoryRepositoryMock.Object,
            brandRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Category.Inactive", result.Error.Code);

        productRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Product>(), CancellationToken.None), Times.Never());

        unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(CancellationToken.None), Times.Never());
    }
}