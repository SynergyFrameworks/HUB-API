using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var configuration = context.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var authToken = configuration?.GetSection("AuthenticationToken")?.Value;

            if (string.IsNullOrEmpty(authToken) || context.Request.Path.Value.StartsWith("/swagger"))
            {
                await _next.Invoke(context);
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Bearer") && authHeader.Length > 7)
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                if (token == authToken)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
