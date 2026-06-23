using Serilog.Context;

namespace EmployeeApi.Api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const string headerName = "X-Correlation-ID";

            if (!context.Request.Headers.TryGetValue(
                headerName,
                out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Response.Headers[headerName] = correlationId;

            using (LogContext.PushProperty(
                "CorrelationId",
                correlationId.ToString()))
            {
                await _next(context);
            }
        }


    }
}
