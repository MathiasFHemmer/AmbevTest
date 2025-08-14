
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

public static class ValidationExtensions
{
    public static async Task ThrowIfInvalid(this Task<ValidationResult> validation)
    {
        var result = await validation;
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}