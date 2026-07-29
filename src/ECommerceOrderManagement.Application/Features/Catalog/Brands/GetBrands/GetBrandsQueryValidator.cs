using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrands;

public sealed class GetBrandsQueryValidator : AbstractValidator<GetBrandsQuery>
{
    private const int MaximumSearchTermLength = 200;

    public GetBrandsQueryValidator()
    {
        RuleFor(query => query.SearchTerm)
            .MaximumLength(MaximumSearchTermLength)
            .WithErrorCode("Brands.SearchTerm.MaxLength")
            .WithMessage($"Search term cannot exceed {MaximumSearchTermLength} characters.");
    }
}