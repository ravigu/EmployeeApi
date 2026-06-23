using EmployeeApi.Application.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace EmployeeApi.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new
                {
                    Message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            catch (UnauthorizedException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.Unauthorized;

                var response = new
                {
                    Message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            catch (NotFoundException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var response = new
                {
                    Message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ValidationException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new
                {
                    Errors = ex.Errors.Select(x => x.ErrorMessage)
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (ConflictException ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.Conflict; // 409

                var response = new
                {
                    Message = ex.Message
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    Message = ex.Message
                };
                _logger.LogError(
                   ex,
                   "Unhandled exception occurred");
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }

        } 
    }
}
