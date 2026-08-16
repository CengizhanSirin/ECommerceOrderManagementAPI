using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses.UpdateAddress;

internal sealed class UpdateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser) : ICommandHandler<UpdateAddressCommand>
{
    public async Task<Result> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAndUserIdAsync(command.AddressId, currentUser.UserId, cancellationToken);

        if (address is null)
        {
            return Result.Failure(AddressErrors.NotFound(command.AddressId));
        }

        address.Update(
            command.Title,
            command.FullName,
            command.PhoneNumber,
            command.Country,
            command.City,
            command.District,
            command.PostalCode,
            command.AddressLine);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}