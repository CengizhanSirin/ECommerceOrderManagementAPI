using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Messaging;
using ECommerceOrderManagement.Application.Common.Results;
using ECommerceOrderManagement.Domain.Addresses;

namespace ECommerceOrderManagement.Application.Features.Addresses.CreateAddress;

internal sealed class CreateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : ICommandHandler<CreateAddressCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var hasAnyAddress = await addressRepository.ExistsForUserAsync(userId, cancellationToken);

        var shouldBeDefault = !hasAnyAddress || command.IsDefault;

        if (hasAnyAddress && command.IsDefault)
        {
            var currentDefaultAddress = await addressRepository.GetDefaultByUserIdAsync(userId, cancellationToken);

            currentDefaultAddress?.RemoveAsDefault();
        }

        var address = Address.Create(
            userId,
            command.Title,
            command.FullName,
            command.PhoneNumber,
            command.Country,
            command.City,
            command.District,
            command.PostalCode,
            command.AddressLine,
            shouldBeDefault);

        await addressRepository.AddAsync(address, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(address.Id);
    }
}