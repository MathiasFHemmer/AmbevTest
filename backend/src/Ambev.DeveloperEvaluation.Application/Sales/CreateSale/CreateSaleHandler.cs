using AutoMapper;
using MediatR;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
/// <summary>
/// Handler for processing CreateSaleHandler requests
/// </summary>
public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateSaleHandler()
    {
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // TODO:
        // Implement sale creation logic
        throw new NotImplementedException($"{nameof(Handle)} not implemented!");
    }
}
