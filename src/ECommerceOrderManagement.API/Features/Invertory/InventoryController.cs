using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Invertory.GetInventoryList;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Invertory;

[Route("api/inventory")]
public sealed class InventoryController(ISender sender) : BaseApiController
{
    [HttpGet]
    [ProducesResponseType<PagedResult<GetInventoryListItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetInventoryList([FromQuery] GetInventoryListRequest request, CancellationToken cancellationToken)
    {
        var query = new GetInventoryListQuery(
            request.Page,
            request.PageSize,
            request.Search,
            request.IsLowStock,
            request.IsOutOfStock,
            request.SortBy,
            request.SortDirection);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
}