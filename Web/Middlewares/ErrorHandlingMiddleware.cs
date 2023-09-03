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
            string userMessage;
            string message;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(Common.Exceptions.KeyNotFoundException))
            {
                userMessage = "ApiKey not found";
                message = userMessage;
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(Common.Exceptions.BusinessLogicException))
            {
                userMessage = exception.Message;
                message = exception.Message;
                statusCode = HttpStatusCode.PreconditionFailed;
            }
            else if (exceptionType == typeof(Common.Exceptions.InvalidModelException))
            {
                userMessage = exception.Message;
                message = exception.Message;
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                userMessage = "A server error has occured.";
                message = exception.Message;
            }
            _logger.LogError(exception, message);

            Common.Dtos.BaseResponseDto responseDto = new Common.Dtos.BaseResponseDto();
            responseDto.SetError(userMessage);

            var exceptionResult = JsonSerializer.Serialize(responseDto);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
