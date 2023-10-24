using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Filtr zajišťuje přidání hlavičky tokenu
/// </summary>
public class RequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();
       
        operation.Parameters.Add(new OpenApiParameter()
        {
             
            Name = "Auth",
            AllowEmptyValue = false,
           
            Schema = new OpenApiSchema() { Type = "string" },
            In =  ParameterLocation.Header,  
            Required = true,
            Example = new OpenApiString("Bearer ....")
        });
    }
}