using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultAddress;

internal sealed class SetDefaultAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : ICommandHandler<SetDefaultAddressCommand>
{
    public async Task<Result> Handle(SetDefaultAddressCommand command, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAndUserIdAsync(command.AddressId, currentUser.UserId, cancellationToken);

        if (address is null)
        {
            return Result.Failure(AddressErrors.NotFound(command.AddressId));
        }

        if (address.IsDefault)
        {
            return Result.Success();
        }

        var currentDefaultAddress = await addressRepository.GetDefaultByUserIdAsync(currentUser.UserId, cancellationToken);

        currentDefaultAddress?.RemoveAsDefault();

        address.SetAsDefault();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}