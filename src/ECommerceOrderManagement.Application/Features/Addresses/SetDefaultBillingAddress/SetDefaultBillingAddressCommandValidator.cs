using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultBillingAddress;

public sealed class SetDefaultBillingAddressCommandValidator : AbstractValidator<SetDefaultBillingAddressCommand>
{
    public SetDefaultBillingAddressCommandValidator()
    {
        RuleFor(command => command.AddressId)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
            .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);
    }
}