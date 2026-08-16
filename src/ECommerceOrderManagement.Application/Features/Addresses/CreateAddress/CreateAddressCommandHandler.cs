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
        var address = Address.Create(
            currentUser.UserId,
            command.Title,
            command.FullName,
            command.PhoneNumber,
            command.Country,
            command.City,
            command.District,
            command.PostalCode,
            command.AddressLine);


        await addressRepository.AddAsync(address, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(address.Id);
    }
}