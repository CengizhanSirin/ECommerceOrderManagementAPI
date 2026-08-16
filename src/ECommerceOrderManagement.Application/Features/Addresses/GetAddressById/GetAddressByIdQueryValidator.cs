using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.GetAddressById;

public sealed class GetAddressByIdQueryValidator:AbstractValidator<GetAddressByIdQuery>
{
    public GetAddressByIdQueryValidator()
    {
        RuleFor(query => query.AddressId)
           .NotEmpty()
           .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
           .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);
    }
}