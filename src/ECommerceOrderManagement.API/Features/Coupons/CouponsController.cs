using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Coupons.CreateCoupon;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Coupons;

[Authorize(Roles = ApplicationRoles.Admin)]
[Route("api/coupons")]
public sealed class CouponsController(ISender sender) : BaseApiController
{


    [HttpPost]
    [ProducesResponseType<CreateCouponResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCouponCommand(
            request.Code,
            request.DiscountType,
            request.DiscountValue,
            request.MinimumOrderAmount,
            request.StartsAtUtc,
            request.EndsAtUtc,
            request.UsageLimit,
            request.UsageLimitPerUser);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(
            result,
            response => response,
            response => $"/api/coupons/{response.Id}");
    }



}