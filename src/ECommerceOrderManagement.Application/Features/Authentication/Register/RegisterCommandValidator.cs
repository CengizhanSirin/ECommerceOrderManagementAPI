using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Authentication.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.FirstNameRequiredCode)
            .WithMessage(AuthenticationValidationErrors.FirstNameRequiredMessage)

            .MaximumLength(100)
            .WithErrorCode(AuthenticationValidationErrors.FirstNameTooLongCode)
            .WithMessage(AuthenticationValidationErrors.FirstNameTooLongMessage);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.LastNameRequiredCode)
            .WithMessage(AuthenticationValidationErrors.LastNameRequiredMessage)

            .MaximumLength(100)
            .WithErrorCode(AuthenticationValidationErrors.LastNameTooLongCode)
            .WithMessage(AuthenticationValidationErrors.LastNameTooLongMessage);

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
            .WithMessage(AuthenticationValidationErrors.PasswordRequiredMessage)

            .MinimumLength(8)
            .WithErrorCode(AuthenticationValidationErrors.PasswordTooShortCode)
            .WithMessage(AuthenticationValidationErrors.PasswordTooShortMessage)

            .Must(password => password.Any(char.IsUpper))
            .WithErrorCode(AuthenticationValidationErrors.PasswordUppercaseRequiredCode)
            .WithMessage(AuthenticationValidationErrors.PasswordUppercaseRequiredMessage)

            .Must(password => password.Any(char.IsLower))
            .WithErrorCode(AuthenticationValidationErrors.PasswordLowercaseRequiredCode)
            .WithMessage(AuthenticationValidationErrors.PasswordLowercaseRequiredMessage)

            .Must(password => password.Any(char.IsDigit))
            .WithErrorCode(AuthenticationValidationErrors.PasswordDigitRequiredCode)
            .WithMessage(AuthenticationValidationErrors.PasswordDigitRequiredMessage)

            .Must(password => password.Any(character => !char.IsLetterOrDigit(character)))
            .WithErrorCode(AuthenticationValidationErrors.PasswordSpecialCharacterRequiredCode)
            .WithMessage(AuthenticationValidationErrors.PasswordSpecialCharacterRequiredMessage);

        RuleFor(command => command.ConfirmPassword)
            .NotEmpty()
            .WithErrorCode(AuthenticationValidationErrors.ConfirmPasswordRequiredCode)
            .WithMessage(AuthenticationValidationErrors.ConfirmPasswordRequiredMessage)

            .Equal(command => command.Password)
            .WithErrorCode(AuthenticationValidationErrors.PasswordsDoNotMatchCode)
            .WithMessage(AuthenticationValidationErrors.PasswordsDoNotMatchMessage);
    }
}