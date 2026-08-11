using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Authentication.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.EmailRequiredCode)
            .WithMessage(AuthenticationValidationErrors.EmailRequiredMessage)

            .EmailAddress()
            .WithErrorCode(AuthenticationValidationErrors.EmailInvalidCode)
            .WithMessage(AuthenticationValidationErrors.EmailInvalidMessage);

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.PasswordRequiredCode)
            .WithMessage(AuthenticationValidationErrors.PasswordRequiredMessage);
    }
}