using System.Net;
using System.Text.Json;

namespace Web.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(Common.Exceptions.KeyNotFoundException))
            {
                message = "ApiKey not found";
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(Common.Exceptions.BusinessLogicException))
            {
                message = exception.Message;
                statusCode = HttpStatusCode.PreconditionFailed;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message;
            }
            _logger.LogError(exception, message);

            var exceptionResult = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
