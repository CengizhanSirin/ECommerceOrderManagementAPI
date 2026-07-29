using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;

public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(query => query.CategoryId)
         .NotEmpty()
         .WithErrorCode(CategoryValidationErrors.IdRequiredCode)
         .WithMessage(CategoryValidationErrors.IdRequiredMessage);

    }
}