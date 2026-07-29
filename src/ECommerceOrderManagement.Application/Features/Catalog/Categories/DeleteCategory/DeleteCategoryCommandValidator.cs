using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(command => command.CategoryId)
            .NotEmpty()
            .WithErrorCode(CategoryValidationErrors.IdRequiredCode)
            .WithMessage(CategoryValidationErrors.IdRequiredMessage);
    }
}