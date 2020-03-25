using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using StackExchange.Profiling;
using System.Threading.Tasks;
using Hub.Transactions.WebAPI.Logs;
using Serilog.Context;

namespace Hub.Transactions.WebAPI.Middleware
{
    public class LogMiddleware
    {
        private readonly IMasker _masker;
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next, IMasker masker)
        {
            _next = next;
            _masker = masker;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.StartsWith("/swagger"))
            {
                await _next.Invoke(context);
            }
            else
            {
                var requestId = Guid.NewGuid().ToString();
                var correlationId = string.IsNullOrEmpty(context.Request.Headers["X-Correlation-ID"].ToString())
                    ? requestId
                    : context.Request.Headers["X-Correlation-ID"].ToString();

                LogContext.PushProperty("RequestId", requestId);
                LogContext.PushProperty("CorrelationId", correlationId);

                var request = new
                {
                    Path = context.Request.Path.Value,
                    context.Request.Method,
                    context.Request.Headers,
                    QueryParams = context.Request.Query,
                    QueryString = context.Request.QueryString.Value,
                    context.Request.ContentType,
                    context.Request.ContentLength,
                    context.Request.Scheme,
                    Host = context.Request.Host.Value,
                    Content = GetContent(context)
                };

                var maskedRequest = _masker.Mask(request);

                Log.ForContext("RequestDetail", maskedRequest, true)
                    .Information("Request started: {Method:l} {path}", request.Method, context.Request.Path);

                var sw = new Stopwatch();

                sw.Start();

                if (Log.IsEnabled(LogEventLevel.Verbose))
                {
                    MiniProfiler.Start();
                }

                await _next.Invoke(context);

                sw.Stop();

                if (MiniProfiler.Current != null)
                {
                    MiniProfiler.Stop();

                    Log.ForContext("Profiler", MiniProfiler.Current, true)
                        .Verbose("Request profiler: {Method:l} {path}",
                            context.Request.Method,
                            context.Request.Path);
                }

                var response = new
                {
                    context.Response.StatusCode,
                    context.Response.Headers,
                    context.Response.ContentType
                };

                var httpDetail = new
                {
                    Request = request,
                    Response = response,
                    DurationInMilliseconds = sw.ElapsedMilliseconds
                };

                var maskedHTTPDetail = _masker.Mask(httpDetail);

                Log.ForContext("HTTPDetail", maskedHTTPDetail, true)
                    .Information("Request completed: {Method:l} {path} [{StatusCode} {StatusDescription:l}] ({DurationInMilliseconds:n0} ms)",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        ((HttpStatusCode) context.Response.StatusCode).ToString(),
                        httpDetail.DurationInMilliseconds);
            }
        }

        private string GetContent(HttpContext context)
        {
            if (context.Request.ContentLength > 0)
            {
                context.Request.Body.Position = 0;

                var body = new StreamReader(context.Request.Body).ReadToEnd();

                context.Request.Body.Position = 0;

                return body;
            }

            return string.Empty;
        }
    }
}
