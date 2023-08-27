using Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Web.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly ILogger<ApiKeyMiddleware> logger;
        private readonly RequestDelegate next;
        private readonly AppSettings appSettings;
        
        public ApiKeyMiddleware(ILogger<ApiKeyMiddleware> logger, RequestDelegate next, IOptions<AppSettings> options)
        {
            this.logger = logger;
            this.next = next;
            appSettings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string apiKeyHeader = appSettings.Security.KeyHeader;
            if (!context.Request.Headers.TryGetValue(apiKeyHeader, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await WriteAsync(context, "Api Key was not provided");
                return;
            }
            var apiKey = appSettings.Security.ApiKey;
            if (!apiKey.Equals(extractedApiKey))
            {
                await WriteAsync(context, "Unauthorized client");
                return;
            }
            await next(context);
        }

        private async Task WriteAsync(HttpContext context, string message)
        {
            Common.Dtos.BaseResponseDto responseDto = new Common.Dtos.BaseResponseDto();
            responseDto.SetError(message);

            logger.LogError(message);

            var exceptionResult = JsonSerializer.Serialize(responseDto);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;

            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
