using Serilog;
using System.Diagnostics;

namespace EmployeeApi.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            Log.Information(
                "Request Started: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            stopwatch.Stop();

            Log.Information(
                "Request Finished: {Method} {Path} StatusCode:{StatusCode} Time:{ElapsedMilliseconds}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
