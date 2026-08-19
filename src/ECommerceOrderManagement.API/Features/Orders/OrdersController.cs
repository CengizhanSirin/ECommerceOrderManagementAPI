using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Orders.CancelOrder;
using ECommerceOrderManagement.API.Features.Orders.CreateOrder;
using ECommerceOrderManagement.API.Features.Orders.GetOrders;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Orders.CancelOrder;
using ECommerceOrderManagement.Application.Features.Orders.CreateOrder;
using ECommerceOrderManagement.Application.Features.Orders.DeliverOrder;
using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;
using ECommerceOrderManagement.Application.Features.Orders.GetOrders;
using ECommerceOrderManagement.Application.Features.Orders.ProcessOrder;
using ECommerceOrderManagement.Application.Features.Orders.ShipOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Orders;

[Route("api/orders")]
public sealed class OrdersController(ISender sender) : BaseApiController
{
    [Authorize(Roles = ApplicationRoles.Customer)]
    [HttpPost]
    [ProducesResponseType<CreateOrderResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.ShippingAddressId, request.BillingAddressId);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, response => response, response => $"/api/orders/{response.OrderId}");
    }



    [HttpGet("{orderId:guid}")]
    [ProducesResponseType<GetOrderByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(orderId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpGet]
    [ProducesResponseType<PagedResult<GetOrdersItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetOrdersQuery(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.CustomerId,
            request.Status,
            request.SortBy,
            request.SortDirection);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPost("{orderId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelOrder([FromRoute] Guid orderId, [FromBody] CancelOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(orderId, request.Reason);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPost("{orderId:guid}/process")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ProcessOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var command = new ProcessOrderCommand(orderId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPost("{orderId:guid}/ship")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ShipOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var command = new ShipOrderCommand(orderId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPost("{orderId:guid}/deliver")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeliverOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var command = new DeliverOrderCommand(orderId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}