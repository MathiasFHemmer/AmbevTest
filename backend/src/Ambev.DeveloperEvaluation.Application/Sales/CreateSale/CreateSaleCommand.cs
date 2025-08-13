using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// including sale number, costumer id and branch id. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public sealed class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or Sets the Sale Number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or Sets the Customer Id
    /// </summary>
    public Guid CustomerId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or Sets the Branch Id
    /// </summary>
    public Guid BranchId { get; set; } = Guid.Empty;
    
    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}