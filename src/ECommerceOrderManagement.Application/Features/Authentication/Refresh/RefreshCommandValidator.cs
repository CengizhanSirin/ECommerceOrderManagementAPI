using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Authentication.Refresh;

public sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.RefreshTokenRequiredCode)
            .WithMessage(AuthenticationValidationErrors.RefreshTokenRequiredMessage);
    }
}