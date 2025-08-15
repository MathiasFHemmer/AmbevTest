using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new Sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        await new CreateSaleRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Adds a new Sale Item to the Sale
    /// </summary>
    /// <param name="request">The sale item creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item identifier</returns>
    [HttpPost("{SaleId:guid}/Items")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSaleItem([FromRoute] Guid SaleId, [FromBody] AddSaleItemRequest request, CancellationToken cancellationToken)
    {
        await new AddSaleItemRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        if (SaleId == Guid.Empty)
            throw new ValidationException($"{nameof(SaleId)} must be present!");

        var command = _mapper.Map<AddSaleItemCommand>(request);
        command.SaleId = SaleId;
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<AddSaleItemResponse>
        {
            Success = true,
            Message = "Sale Item created successfully",
            Data = _mapper.Map<AddSaleItemResponse>(response)
        });
    }

    /// <summary>
    /// Adds a new Sale Item to the Sale
    /// </summary>
    /// <param name="request">The sale item creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item identifier</returns>
    [HttpDelete("{SaleId:guid}/Item/{ProductId:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSaleItem([FromRoute] Guid SaleId, Guid ProductId, CancellationToken cancellationToken)
    {
        var request = new CancelSaleItemRequest
        {
            SaleId = SaleId,
            ProductId = ProductId
        };
        await new CancelSaleItemRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        var command = _mapper.Map<CancelSaleItemCommand>(request);

        await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Product cancelled from sale successfully!"
        });
    }
}