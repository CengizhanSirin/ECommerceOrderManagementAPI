using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Brands.CreateBrand;

public sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private const int MaximumNameLength = 200;
    private const int MaximumSlugLength = 200;
    private const int MaximumDescriptionLength = 2000;
    private const int MaximumLogoUrlLength = 2048;

    public CreateBrandCommandValidator()
    {
        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithErrorCode(BrandValidationErrors.NameRequiredCode)
            .WithMessage(BrandValidationErrors.NameRequiredMessage)
            .MaximumLength(MaximumNameLength)
            .WithErrorCode(BrandValidationErrors.NameMaxLengthCode)
            .WithMessage($"Brand name cannot exceed {MaximumNameLength} characters.");

        RuleFor(command => command.Slug)
            .Cascade(CascadeMode.Stop)
            .Must(slug => !string.IsNullOrWhiteSpace(slug))
            .WithErrorCode(BrandValidationErrors.SlugRequiredCode)
            .WithMessage(BrandValidationErrors.SlugRequiredMessage)
            .MaximumLength(MaximumSlugLength)
            .WithErrorCode(BrandValidationErrors.SlugMaxLengthCode)
            .WithMessage($"Brand slug cannot exceed {MaximumSlugLength} characters.")
            .Matches("^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$")
            .WithErrorCode(BrandValidationErrors.SlugInvalidFormatCode)
            .WithMessage(BrandValidationErrors.SlugInvalidFormatMessage);

        RuleFor(command => command.Description)
            .MaximumLength(MaximumDescriptionLength)
            .WithErrorCode(BrandValidationErrors.DescriptionMaxLengthCode)
            .WithMessage($"Brand description cannot exceed {MaximumDescriptionLength} characters.");

        RuleFor(command => command.LogoUrl)
            .MaximumLength(MaximumLogoUrlLength)
            .WithErrorCode(BrandValidationErrors.LogoUrlMaxLengthCode)
            .WithMessage($"Brand logo URL cannot exceed {MaximumLogoUrlLength} characters.");

        RuleFor(command => command.LogoUrl)
            .Must(BeAValidHttpUrl)
            .When(command => !string.IsNullOrWhiteSpace(command.LogoUrl))
            .WithErrorCode(BrandValidationErrors.LogoUrlInvalidCode)
            .WithMessage(BrandValidationErrors.LogoUrlInvalidMessage);
    }

    private static bool BeAValidHttpUrl(string? logoUrl)
    {
        return Uri.TryCreate(logoUrl, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
