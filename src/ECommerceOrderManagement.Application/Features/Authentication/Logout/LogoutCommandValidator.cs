using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Authentication.Logout;

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.RefreshTokenRequiredCode)
            .WithMessage(AuthenticationValidationErrors.RefreshTokenRequiredMessage);
    }
}