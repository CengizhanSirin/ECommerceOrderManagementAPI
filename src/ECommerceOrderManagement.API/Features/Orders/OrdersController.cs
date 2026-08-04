using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Orders.CreateOrder;
using ECommerceOrderManagement.Application.Features.Orders.CreateOrder;
using ECommerceOrderManagement.Application.Features.Orders.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Orders;

[Route("api/orders")]
public sealed class OrdersController(ISender sender) : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<CreateOrderResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.CustomerId, MapAddress(request.ShippingAddress), MapAddress(request.BillingAddress),
            request.Items.Select(MapItem).ToArray());

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

    private static CreateOrderAddress MapAddress(CreateOrderAddressRequest address)
    {
        return new CreateOrderAddress(address.FullName, address.PhoneNumber, address.Country, address.City, address.District, address.PostalCode, address.AddressLine);
    }

    private static CreateOrderItem MapItem(CreateOrderItemRequest item)
    {
        return new CreateOrderItem(item.ProductId, item.Quantity);
    }
}