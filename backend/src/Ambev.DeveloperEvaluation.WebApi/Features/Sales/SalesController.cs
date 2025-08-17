using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.ORM.Pagination;
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
    /// Cancel a Sale Item on the Sale
    /// </summary>
    /// <param name="SaleId">The sale to be looked in for the item to cancel</param>
    /// <param name="ProductId">The product id from the sale item to be canceled</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success if item is deleted</returns>
    [HttpDelete("{SaleId:guid}/Item/{ProductId:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid SaleId, Guid ProductId, CancellationToken cancellationToken)
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

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSaleResponseEntry>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListSales([PaginationFromQuery] PaginateRequest pagination, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ListSaleCommand>(pagination);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponseWithData<ListSaleResponse>
        {
            Success = true,
            Data = _mapper.Map<ListSaleResponse>(result),
        });
    }
}