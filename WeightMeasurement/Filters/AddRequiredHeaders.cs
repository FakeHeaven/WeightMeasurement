using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WeightMeasurement.Filters
{
    public class AddRequiredHeaders : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            if (!(apiDescription.RelativePath == "api/token"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Basic Authentication Header (Basic XXXXXXXX)",
                    Schema = new OpenApiSchema() { Type = "String" },
                    Required = true
                });
            }

        }
    }
}
