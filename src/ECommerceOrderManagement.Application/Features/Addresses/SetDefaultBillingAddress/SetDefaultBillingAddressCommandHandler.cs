using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Addresses;

namespace ECommerceOrderManagement.Application.Features.Addresses.SetDefaultBillingAddress;

internal sealed class SetDefaultBillingAddressCommandHandler(IAddressRepository addressRepository, IUserAddressPreferenceRepository preferenceRepository, IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : ICommandHandler<SetDefaultBillingAddressCommand>
{
    public async Task<Result> Handle(SetDefaultBillingAddressCommand command, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetByIdAndUserIdAsync(command.AddressId, currentUser.UserId, cancellationToken);

        if (address is null)
        {
            return Result.Failure(AddressErrors.NotFound(command.AddressId));
        }

        var preference = await preferenceRepository.GetByUserIdAsync(currentUser.UserId, cancellationToken);

        if (preference is null)
        {
            preference = UserAddressPreference.Create(currentUser.UserId);

            await preferenceRepository.AddAsync(preference, cancellationToken);
        }

        if (preference.DefaultBillingAddressId == address.Id)
        {
            return Result.Success();
        }

        preference.SetDefaultBillingAddress(address.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}