using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Addresses.DeleteAddress;

internal sealed class DeleteAddressCommandHandler(IAddressRepository addressRepository, IUserAddressPreferenceRepository userAddressPreferenceRepository, IUnitOfWork unitOfWork,
    ICurrentUser currentUser, TimeProvider timeProvider)
    : ICommandHandler<DeleteAddressCommand>
{
    public async Task<Result> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAndUserIdAsync(command.AddressId, currentUser.UserId, cancellationToken);

        if (address is null)
        {
            return Result.Failure(AddressErrors.NotFound(command.AddressId));
        }

        var preference = await userAddressPreferenceRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        preference?.ClearAddress(address.Id);

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        address.Delete(utcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}