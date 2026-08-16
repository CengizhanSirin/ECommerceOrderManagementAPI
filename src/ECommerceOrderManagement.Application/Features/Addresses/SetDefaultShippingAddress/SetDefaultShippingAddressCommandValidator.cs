using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultShippingAddress;

public sealed class SetDefaultShippingAddressCommandValidator : AbstractValidator<SetDefaultShippingAddressCommand>
{
    public SetDefaultShippingAddressCommandValidator()
    {
        RuleFor(command => command.AddressId)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
            .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);
    }
}