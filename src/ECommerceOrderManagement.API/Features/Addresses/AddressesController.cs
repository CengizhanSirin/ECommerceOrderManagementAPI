using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Addresses.CreateAddress;
using ECommerceOrderManagement.API.Features.Addresses.UpdateAddress;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.Addresses;
using ECommerceOrderManagement.Application.Features.Addresses.CreateAddress;
using ECommerceOrderManagement.Application.Features.Addresses.DeleteAddress;
using ECommerceOrderManagement.Application.Features.Addresses.GetAddressById;
using ECommerceOrderManagement.Application.Features.Addresses.GetAddresses;
using ECommerceOrderManagement.Application.Features.Addresses.UpdateAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Addresses;

[Authorize(Roles = ApplicationRoles.Customer)]
[Route("api/addresses")]
public sealed class AddressesController(ISender sender) : BaseApiController
{

    [HttpPost]
    [ProducesResponseType<CreateAddressResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAddressCommand(
            request.Title,
            request.FullName,
            request.PhoneNumber,
            request.Country,
            request.City,
            request.District,
            request.PostalCode,
            request.AddressLine,
            request.IsDefault);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(result,
            addressId => new CreateAddressResponse
            {
                Id = addressId
            },
            response => $"/api/addresses/{response.Id}");
    }



    [HttpGet("{addressId:guid}")]
    [ProducesResponseType<AddressReadModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAddressById([FromRoute] Guid addressId, CancellationToken cancellationToken)
    {
        var query = new GetAddressByIdQuery(addressId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<AddressReadModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAddresses(CancellationToken cancellationToken)
    {
        var query = new GetAddressesQuery();

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPut("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid addressId, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAddressCommand(addressId, request.Title, request.FullName, request.PhoneNumber, request.Country,
            request.City, request.District, request.PostalCode, request.AddressLine);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpDelete("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAddress([FromRoute] Guid addressId, CancellationToken cancellationToken)
    {
        var command = new DeleteAddressCommand(addressId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}