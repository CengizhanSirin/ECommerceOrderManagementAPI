using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.DeleteAddress;

public sealed class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidator()
    {
        RuleFor(command => command.AddressId)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
            .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);
    }
}