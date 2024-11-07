using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.Products.Commands.Create;
using WebApplication1.Application.Products.Commands.Delete;
using WebApplication1.Application.Products.Commands.Update;
using WebApplication1.Application.Products.Queries;
using WebApplication1.Authorization;
using WebApplication1.Controllers.Requests;
using WebApplication1.Extensions;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ApplicationController
{
    //[Permission("products.create")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        [FromServices] CreateProductHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateProductCommand(
            request.ProductName,
            request.Type,
            request.Description,
            request.FunctionalRequirements,
            request.Price,
            request.ExpirationDate);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Created("", result.Value);
    }
    
    //[Permission("products.delete")]
    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid productId,
        [FromServices] DeleteProductHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(productId, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    //[Permission("products.update")]
    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductRequest request,
        [FromServices] UpdateProductHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateProductCommand(
            productId,
            request.ProductName,
            request.Type,
            request.Description,
            request.FunctionalRequirements,
            request.Price,
            request.ExpirationDate);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [Permission("products.get")]
    [HttpGet]
    public async Task<IActionResult> GetWithPagination(
        [FromQuery] GetProductsWithPaginationRequest request,
        [FromServices] GetProductsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new GetProductsWithPaginationQuery(
            request.Page,
            request.PageSize,
            request.OrderBy);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}