using Hub.Transactions.WebAPI.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Hub.Transactions.WebAPI.Filters
{
    public class MethodNamesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var action = context.ApiDescription.HttpMethod.ToFirstOnlyUpper();
            var name = string.Join(string.Empty, context.ApiDescription.RelativePath.Split('/').Select(x => x.ToFirstUpper()));

            operation.OperationId = action + name;
        }
    }
}
