using NET.API.DTOs;
using NET.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace NET.API.Middlewares
{
    /// <summary>
    /// Global hata yakalama middleware'i.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            object response;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ValidationErrorResponse(validationEx.Errors);
                    break;

                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new ErrorResponse(badRequestEx.Message);
                    break;

                case KeyNotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    response = new ErrorResponse(notFoundEx.Message ?? "Kayıt bulunamadı");
                    break;

                default:
                    _logger.LogError(exception, "Beklenmeyen bir hata oluştu");
                    response = new ErrorResponse("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
