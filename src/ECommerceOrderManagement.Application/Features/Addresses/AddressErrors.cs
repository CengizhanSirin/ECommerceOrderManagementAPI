using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses;

public static class AddressErrors
{
    public static Error NotFound(Guid addressId)
    {
        return Error.NotFound( "Address.NotFound", $"Address with ID '{addressId}' was not found.");
    }
}