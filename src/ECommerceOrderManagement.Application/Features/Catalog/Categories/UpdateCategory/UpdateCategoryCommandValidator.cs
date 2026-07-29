using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Categories.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private const int MaximumNameLength = 200;
    private const int MaximumSlugLength = 200;
    private const int MaximumDescriptionLength = 2000;
    private const int MaximumImageUrlLength = 2048;

    public UpdateCategoryCommandValidator()
    {
        RuleFor(command => command.CategoryId)
            .NotEmpty()
            .WithErrorCode(CategoryValidationErrors.IdRequiredCode)
            .WithMessage(CategoryValidationErrors.IdRequiredMessage);

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithErrorCode(CategoryValidationErrors.NameRequiredCode)
            .WithMessage(CategoryValidationErrors.NameRequiredMessage)
            .MaximumLength(MaximumNameLength)
            .WithErrorCode(CategoryValidationErrors.NameMaxLengthCode)
            .WithMessage($"Category name cannot exceed {MaximumNameLength} characters.");


        RuleFor(command => command.Slug)
            .Cascade(CascadeMode.Stop)
            .Must(slug => !string.IsNullOrWhiteSpace(slug))
            .WithErrorCode(CategoryValidationErrors.SlugRequiredCode)
            .WithMessage(CategoryValidationErrors.SlugRequiredMessage)
            .MaximumLength(MaximumSlugLength)
            .WithErrorCode(CategoryValidationErrors.SlugMaxLengthCode)
            .WithMessage($"Category slug cannot exceed {MaximumSlugLength} characters.")
            .Matches("^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$")
            .WithErrorCode(CategoryValidationErrors.SlugInvalidFormatCode)
            .WithMessage(CategoryValidationErrors.SlugInvalidFormatMessage);

        RuleFor(command => command.Description)
            .MaximumLength(MaximumDescriptionLength)
            .WithErrorCode(CategoryValidationErrors.DescriptionMaxLengthCode)
            .WithMessage($"Category description cannot exceed {MaximumDescriptionLength} characters.");

        RuleFor(command => command.ImageUrl)
            .MaximumLength(MaximumImageUrlLength)
            .WithErrorCode(CategoryValidationErrors.ImageUrlMaxLengthCode)
            .WithMessage($"Category image URL cannot exceed {MaximumImageUrlLength} characters.");

        RuleFor(command => command.ImageUrl)
            .Must(BeAValidHttpUrl)
            .When(command => !string.IsNullOrWhiteSpace(command.ImageUrl))
            .WithErrorCode(CategoryValidationErrors.ImageUrlInvalidCode)
            .WithMessage(CategoryValidationErrors.ImageUrlInvalidMessage);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(CategoryValidationErrors.DisplayOrderInvalidCode)
            .WithMessage(CategoryValidationErrors.DisplayOrderInvalidMessage);
    }

    private static bool BeAValidHttpUrl(string? imageUrl)
    {
        return Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}