using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Addresses.CreateAddress;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.Addresses.CreateAddress;
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
}