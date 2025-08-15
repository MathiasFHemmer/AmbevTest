using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public sealed class ExceptionMiddleware
    {
        protected readonly RequestDelegate _next;
        protected static readonly JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await NotFoundExceptionMiddlewareHandler.HandleException(context, ex, DefaultJsonOptions);
            }
            catch (ValidationException ex)
            {
                await ValidationExceptionMiddlewareHandler.HandleException(context, ex, DefaultJsonOptions);
            }
            catch (DomainException ex)
            {
                await DomainExceptionMiddlewareHandler.HandleException(context, ex, DefaultJsonOptions);
            }
        }
    }
}
