using System.Net;
using System.Text.Json;
using System.Web;
using Web.User.Models;

namespace Web.User.Middlewares
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
            if (exceptionType == typeof(Common.Exceptions.BusinessLogicException))
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

            ApplicationErrorModel responseDto = new ApplicationErrorModel();
            responseDto.StatusCode = (int) statusCode;
            responseDto.SetError(userMessage);

            var exceptionResult = JsonSerializer.Serialize(responseDto);

            var errorString = HttpUtility.UrlEncode(exceptionResult);

            context.Response.Redirect($"/Account/Error?error={errorString}");
        }
    }
}
