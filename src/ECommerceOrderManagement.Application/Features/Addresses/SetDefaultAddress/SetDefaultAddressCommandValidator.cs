using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultAddress;

public sealed class SetDefaultAddressCommandValidator : AbstractValidator<SetDefaultAddressCommand>
{
    public SetDefaultAddressCommandValidator()
    {
        RuleFor(command => command.AddressId)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
            .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);
    }
}