namespace EmployeeApi.Api.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}