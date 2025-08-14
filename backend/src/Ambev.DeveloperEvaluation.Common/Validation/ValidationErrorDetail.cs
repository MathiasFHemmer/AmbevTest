using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Validation;

public class ValidationErrorDetail
{
    public string Origin { get; init; } = String.Empty;
    public string Error { get; init; } = string.Empty;
    public string Detail { get; init; } = string.Empty;

    public static explicit operator ValidationErrorDetail(ValidationFailure validationFailure)
    {
        return new ValidationErrorDetail
        {
            Origin = validationFailure.PropertyName,
            Detail = validationFailure.ErrorMessage,
            Error = validationFailure.ErrorCode
        };
    }
}
