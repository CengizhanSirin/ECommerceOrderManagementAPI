using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Coupons.CreateCoupon;
using ECommerceOrderManagement.API.Features.Coupons.GetCoupons;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Coupons.ActivateCoupon;
using ECommerceOrderManagement.Application.Features.Coupons.CreateCoupon;
using ECommerceOrderManagement.Application.Features.Coupons.DeactivateCoupon;
using ECommerceOrderManagement.Application.Features.Coupons.GetCouponById;
using ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;
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



    [HttpGet("{couponId:guid}")]
    [ProducesResponseType<GetCouponByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCouponById([FromRoute] Guid couponId, CancellationToken cancellationToken)
    {
        var query = new GetCouponByIdQuery(couponId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpGet]
    [ProducesResponseType<PagedResult<GetCouponsItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCoupons([FromQuery] GetCouponsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetCouponsQuery(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.DiscountType,
            request.IsActive,
            request.SortBy,
            request.SortDirection);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPost("{couponId:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateCoupon([FromRoute] Guid couponId, CancellationToken cancellationToken)
    {
        var command = new ActivateCouponCommand(couponId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPost("{couponId:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateCoupon([FromRoute] Guid couponId, CancellationToken cancellationToken)
    {
        var command = new DeactivateCouponCommand(couponId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}