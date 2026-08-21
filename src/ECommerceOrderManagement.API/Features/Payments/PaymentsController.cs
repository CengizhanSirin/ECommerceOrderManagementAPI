using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Payments.ProcessPayment;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Payments;

[Authorize(Roles = ApplicationRoles.Customer)]
[Route("api/payments")]
public sealed class PaymentsController(ISender sender) : BaseApiController
{
    [HttpPost("pay-order")]
    [ProducesResponseType<ProcessPaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request, CancellationToken cancellationToken)
    {
        var command = new ProcessPaymentCommand(request.OrderId, request.CardHolderName, request.CardNumber, request.ExpireMonth,
            request.ExpireYear, request.Cvc);

        var result = await sender.Send(command, cancellationToken);

        return HandleResult(result);
    }
}