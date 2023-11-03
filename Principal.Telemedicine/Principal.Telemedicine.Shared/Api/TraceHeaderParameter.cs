using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Filtr zajišťuje přidání hlavičky tokenu
/// </summary>
public class TraceHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();
       
        operation.Parameters.Add(new OpenApiParameter()
        {
             
            Name = "global_trace_key",
            AllowEmptyValue = true,
           
            Schema = new OpenApiSchema() { Type = "string" },
            In =  ParameterLocation.Header,  
            Required = false,
            Example = new OpenApiString("MY_API")
        });
    }
}