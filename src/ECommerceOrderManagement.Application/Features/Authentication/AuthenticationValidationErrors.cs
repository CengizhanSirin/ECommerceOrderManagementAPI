namespace ECommerceOrderManagement.Application.Features.Authentication;

public static class AuthenticationValidationErrors
{
    public const string FirstNameRequiredCode = "Authentication.FirstNameRequired";
    public const string FirstNameRequiredMessage = "First name is required.";

    public const string FirstNameTooLongCode = "Authentication.FirstNameTooLong";
    public const string FirstNameTooLongMessage = "First name cannot exceed 100 characters.";

    public const string LastNameRequiredCode = "Authentication.LastNameRequired";
    public const string LastNameRequiredMessage = "Last name is required.";

    public const string LastNameTooLongCode = "Authentication.LastNameTooLong";
    public const string LastNameTooLongMessage = "Last name cannot exceed 100 characters.";

    public const string EmailRequiredCode = "Authentication.EmailRequired";
    public const string EmailRequiredMessage = "Email is required.";

    public const string EmailInvalidCode = "Authentication.EmailInvalid";
    public const string EmailInvalidMessage = "Email address is invalid.";

    public const string PasswordRequiredCode = "Authentication.PasswordRequired";
    public const string PasswordRequiredMessage = "Password is required.";

    public const string PasswordTooShortCode = "Authentication.PasswordTooShort";
    public const string PasswordTooShortMessage = "Password must be at least 8 characters.";

    public const string PasswordUppercaseRequiredCode = "Authentication.PasswordUppercaseRequired";
    public const string PasswordUppercaseRequiredMessage = "Password must contain at least one uppercase letter.";

    public const string PasswordLowercaseRequiredCode = "Authentication.PasswordLowercaseRequired";
    public const string PasswordLowercaseRequiredMessage = "Password must contain at least one lowercase letter.";

    public const string PasswordDigitRequiredCode = "Authentication.PasswordDigitRequired";
    public const string PasswordDigitRequiredMessage = "Password must contain at least one digit.";

    public const string PasswordSpecialCharacterRequiredCode = "Authentication.PasswordSpecialCharacterRequired";
    public const string PasswordSpecialCharacterRequiredMessage = "Password must contain at least one special character.";

    public const string ConfirmPasswordRequiredCode = "Authentication.ConfirmPasswordRequired";
    public const string ConfirmPasswordRequiredMessage = "Password confirmation is required.";

    public const string PasswordsDoNotMatchCode = "Authentication.PasswordsDoNotMatch";
    public const string PasswordsDoNotMatchMessage = "Password and confirmation password do not match.";
}