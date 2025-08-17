using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class PaginationFromQueryOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.ApiDescription.ParameterDescriptions;
        foreach (var param in parameters)
        {
            if (param.ParameterDescriptor is ControllerParameterDescriptor descriptor &&
                descriptor.ParameterInfo.GetCustomAttribute<PaginationFromQueryAttribute>() != null)
            {
                var toRemove = operation.Parameters.FirstOrDefault(p => p.Name == param.Name);
                if (toRemove != null)
                    operation.Parameters.Remove(toRemove);

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "_page",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema { Type = "integer", Format = "uint32", Default = new OpenApiInteger(1) }
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "_size",
                    In = ParameterLocation.Query,
                    Required = false,
                    Schema = new OpenApiSchema { Type = "integer", Format = "uint32", Default = new OpenApiInteger(10) }
                });
            }
        }
    }
}