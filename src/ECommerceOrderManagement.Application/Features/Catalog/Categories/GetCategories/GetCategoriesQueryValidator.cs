using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    private const int MaximumSearchTermLength = 200;

    public GetCategoriesQueryValidator()
    {
        RuleFor(query => query.SearchTerm)
            .MaximumLength(MaximumSearchTermLength)
            .WithErrorCode(CategoryValidationErrors.CategoriesSearchTermMaxLengthCode)
            .WithMessage($"Search term cannot exceed {MaximumSearchTermLength} characters.");
    }
}