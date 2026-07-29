using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Catalog.Brands.CreateBrand;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.CreateBrand;
using ECommerceOrderManagement.Application.Features.Catalog.Brands.GetBrandById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Catalog.Brands;

[Route("api/brands")]
public sealed class BrandsController(ISender sender) : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<CreateBrandResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateBrandCommand(request.Name, request.Slug, request.Description, request.LogoUrl);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, response => response, response => $"/api/brands/{response.Id}");
    }


    [HttpGet("{brandId:guid}")]
    [ProducesResponseType<GetBrandByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBrandById([FromRoute] Guid brandId, CancellationToken cancellationToken)
    {
        var query = new GetBrandByIdQuery(brandId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
}