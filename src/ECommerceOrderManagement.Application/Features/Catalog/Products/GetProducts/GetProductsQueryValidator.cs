using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    private const int MaximumPageSize = 100;
    private const int MaximumSearchTermLength = 200;
    public GetProductsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0)
            .WithErrorCode(ProductValidationErrors.ProductsPageNumberInvalidCode)
            .WithMessage(ProductValidationErrors.ProductsPageNumberInvalidMessage);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, MaximumPageSize)
            .WithErrorCode(ProductValidationErrors.ProductsPageSizeInvalidCode)
            .WithMessage($"Page size must be between 1 and {MaximumPageSize}.");


        RuleFor(query => query.SearchTerm)
            .MaximumLength(MaximumSearchTermLength)
            .WithErrorCode(ProductValidationErrors.ProductsSearchTermMaxLengthCode)
            .WithMessage($"Search term cannot exceed {MaximumSearchTermLength} characters.");


        RuleFor(query => query.CategoryId)
            .Must(categoryId => !categoryId.HasValue || categoryId.Value != Guid.Empty)
            .WithErrorCode(ProductValidationErrors.ProductsCategoryInvalidCode)
            .WithMessage(ProductValidationErrors.ProductsCategoryInvalidMessage);

        RuleFor(query => query.BrandId)
            .Must(brandId => !brandId.HasValue || brandId.Value != Guid.Empty)
            .WithErrorCode(ProductValidationErrors.BrandInvalidCode)
            .WithMessage(ProductValidationErrors.BrandInvalidMessage);

        RuleFor(query => query.SortBy)
            .IsInEnum()
            .WithErrorCode(ProductValidationErrors.ProductsSortFieldInvalidCode)
            .WithMessage(ProductValidationErrors.ProductsSortFieldInvalidMessage);

        RuleFor(query => query.SortDirection)
            .IsInEnum()
            .WithErrorCode(ProductValidationErrors.ProductsSortDirectionInvalidCode)
            .WithMessage(ProductValidationErrors.ProductsSortDirectionInvalidMessage);
    }
}